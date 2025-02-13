using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.Wallet;
using Galini.Models.Utils;
using Galini.Repository.Interface;
using Galini.Services.Interface;
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

        public async Task<BaseResponse> CreatePaymentUrlRegisterCreator(decimal balance)
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


                string buyerName = user.FullName;
                string buyerPhone = user.Phone;
                string buyerEmail = user.Email;

                Random random = new Random();
                long orderCode = (DateTime.Now.Ticks % 1000000000000000L) * 10 + random.Next(0, 1000);
                var description = "VQRIO123";
                var signatureData = new Dictionary<string, object>
                {
                    { "amount", balance },
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
                    amount: (int)balance,
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


                // Gọi API tạo thanh toán
                var paymentResult = await _payOS.createPaymentLink(paymentData);

                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Tạo link thanh toán thành công",
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
    }
}
