using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Enum;
using Galini.Models.Payload.Request.Deposit;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.Wallet;
using Galini.Models.Utils;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Galini.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;

namespace Galini.Services.Implement
{
    public class WalletService : BaseService<WalletService>, IWalletService
    {
        private readonly PayOSSettings _payOSSettings;
        private readonly PayOS _payOS;
        public WalletService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<WalletService> logger, 
            IMapper mapper, IHttpContextAccessor httpContextAccessor, IOptions<PayOSSettings> settings) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _payOSSettings = settings.Value;
            _payOS = new PayOS(_payOSSettings.ClientId, _payOSSettings.ApiKey, _payOSSettings.ChecksumKey);
        }

        public async Task<BaseResponse> ConfirmWebhook(WebhookType payload)
        {
            string code = payload.code;
            bool success = payload.success;
            var transaction = await _unitOfWork.GetRepository<Models.Entity.Transaction>().SingleOrDefaultAsync(
                predicate: t => t.OrderCode == payload.data.orderCode);
            if (success && code == "00")
            {
                await HandleSuccessPayment(transaction);
            }
            else
            {
                await HandleFailedPayment(transaction);
            }
            return new BaseResponse
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Thành công",
                data = true
            };
        }

        public async Task HandleSuccessPayment(Models.Entity.Transaction transaction)
        {
            transaction.UpdateAt = TimeUtil.GetCurrentSEATime();
            transaction.Status = TransactionStatusEnum.SUCCESS.ToString();
            var wallet = await _unitOfWork.GetRepository<Wallet>().SingleOrDefaultAsync(
                predicate: w => w.Id.Equals(transaction.WalletId));
            if (wallet == null)
            {
                throw new Exception("Không tìm thấy ví người dùng");
            }

            wallet.Balance += transaction.Amount;
            wallet.UpdateAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<Models.Entity.Transaction>().UpdateAsync(transaction);
            _unitOfWork.GetRepository<Wallet>().UpdateAsync(wallet);
            await _unitOfWork.CommitAsync();
        }

        public async Task HandleFailedPayment(Models.Entity.Transaction transaction)
        {
            transaction.UpdateAt = TimeUtil.GetCurrentSEATime();
            transaction.Status = TransactionStatusEnum.FAILED.ToString();
            _unitOfWork.GetRepository<Models.Entity.Transaction>().UpdateAsync(transaction);
            await _unitOfWork.CommitAsync();
        }

        public async Task<BaseResponse> CreatePaymentUrlRegisterCreator(CreateDepositRequest request)
        {
            try
            {
                Guid? id = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
                var user = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                    predicate: u => u.Id.Equals(id) && u.IsActive == true);

                if (user == null)
                {
                    return new BaseResponse()
                    {
                        status = StatusCodes.Status404NotFound.ToString(),
                        message = "Không tìm người dùng với ID này",
                        data = null
                    };
                }

                var wallet = await _unitOfWork.GetRepository<Wallet>().SingleOrDefaultAsync(
                    predicate: w => w.AccountId.Equals(user.Id) && w.IsActive == true);

                if (wallet == null)
                {
                    return new BaseResponse()
                    {
                        status = StatusCodes.Status404NotFound.ToString(),
                        message = "Không tìm thấy ví của người này",
                        data = null
                    };
                }


                string buyerName = user.FullName;
                string buyerPhone = user.Phone;
                string buyerEmail = user.Email;

                Random random = new Random();
                long orderCode = (DateTime.Now.Ticks % 1000000000000000L) * 10 + random.Next(0, 1000);
                var description = "VQRIO123";
                var signatureData = new Dictionary<string, object>
                {
                    { "amount", request.Amount },
                    { "cancelUrl", _payOSSettings.ReturnUrlFail },
                    { "description", description },
                    { "expiredAt", DateTimeOffset.Now.AddMinutes(10).ToUnixTimeSeconds() },
                    { "orderCode", orderCode },
                    { "returnUrl", _payOSSettings.ReturnUrl }
                };

                var sortedSignatureData = new SortedDictionary<string, object>(signatureData);
                var dataForSignature = string.Join("&", sortedSignatureData.Select(p => $"{p.Key}={p.Value}"));
                var signature = ComputeHmacSha256(dataForSignature, _payOSSettings.ChecksumKey);

                DateTimeOffset expiredAt = DateTimeOffset.Now.AddMinutes(10);

                var paymentData = new PaymentData(
                    orderCode: orderCode,
                    amount: (int)request.Amount,
                    description: description,
                    items: null,
                    cancelUrl: _payOSSettings.ReturnUrlFail,
                    returnUrl: _payOSSettings.ReturnUrl,
                    signature: signature,
                    buyerName: buyerName,
                    buyerPhone: buyerPhone,
                    buyerEmail: buyerEmail,

                    buyerAddress: "HCM",
                    expiredAt: (int)expiredAt.ToUnixTimeSeconds()
                );

                var deposit = _mapper.Map<Deposit>(request);
                deposit.Code = orderCode.ToString();
                deposit.AccountId = user.Id;
                await _unitOfWork.GetRepository<Deposit>().InsertAsync(deposit);

                var transaction = new Models.Entity.Transaction()
                {
                    Id = Guid.NewGuid(),
                    Amount = request.Amount,
                    DepositId = deposit.Id,
                    WalletId = wallet.Id,
                    OrderCode = orderCode,
                    IsActive = true,
                    Status = TransactionStatusEnum.PENDING.GetDescriptionFromEnum(),
                    Type = TransactionTypeEnum.DEPOSIT.GetDescriptionFromEnum(),
                    CreateAt = TimeUtil.GetCurrentSEATime(),
                    UpdateAt = TimeUtil.GetCurrentSEATime(),
                };
                await _unitOfWork.GetRepository< Models.Entity.Transaction>().InsertAsync(transaction);

                bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

                var paymentResult = await _payOS.createPaymentLink(paymentData);



                if (isSuccessfully)
                {
                    return new BaseResponse()
                    {
                        status = StatusCodes.Status200OK.ToString(),
                        message = "Tạo link thanh toán thành công",
                        data = paymentResult,
                    };
                }

                return new BaseResponse()
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Tạo link thanh toán thất bại",
                    data = paymentResult,
                };
            }

            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the payment URL.", ex);
            }
        }

        private string? ComputeHmacSha256(string data, string checksumKey)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(checksumKey)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        public async Task<BaseResponse> GetWallet()
        {
            Guid? id = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var user = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: u => u.Id.Equals(id) && u.IsActive == true);

            if (user == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm người dùng với ID này",
                    data = null
                };
            }

            var wallet = await _unitOfWork.GetRepository<Wallet>().SingleOrDefaultAsync(
                selector: w => _mapper.Map<GetWalletResponse>(w),
                predicate: w => w.AccountId.Equals(user.Id) && w.IsActive == true);
            if (wallet == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy ví tiền",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Thông tin ví tiền",
                data = wallet
            };

        }
    }
}
