using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Enum;
using Galini.Models.Payload.Request.Premium;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.Premium;
using Galini.Models.Payload.Response.Transaction;
using Galini.Models.Payload.Response.UserInfo;
using Galini.Models.Utils;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Galini.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Implement
{
    public class PremiumService : BaseService<PremiumService>, IPremiumService
    {
        private readonly IMapper _mapper;
        public PremiumService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<PremiumService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _mapper = mapper;
        }

        public async Task<BaseResponse> BuyPremium(Guid id)
        {
            Guid? userId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var user = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: u => u.Id.Equals(userId) && u.IsActive);
            if (user == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tim thấy người dùng",
                    data = null
                };
            }

            var wallet = await _unitOfWork.GetRepository<Wallet>().SingleOrDefaultAsync(
                predicate: w => w.AccountId.Equals(user.Id) && w.IsActive);
            if (wallet == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy ví người dùng",
                    data = null
                };
            }

            var userInfo = await _unitOfWork.GetRepository<UserInfo>().SingleOrDefaultAsync(
                predicate: u => u.AccountId.Equals(user.Id) && u.IsActive);
            if(userInfo == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin người dùng",
                    data = null
                };
            }

            var premium = await _unitOfWork.GetRepository<Premium>().SingleOrDefaultAsync(
                predicate: p => p.Id.Equals(id) && p.IsActive);
            if (premium == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy gói premium",
                    data = null
                };
            }

            if (wallet.Balance < (decimal)premium.Price)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Số tiền trong ví không đủ để mua gói này",
                    data = null
                };
            }

            userInfo.PremiumId = id;
            _unitOfWork.GetRepository<UserInfo>().UpdateAsync(userInfo);

            wallet.Balance -= (decimal)premium.Price;
            _unitOfWork.GetRepository<Wallet>().UpdateAsync(wallet);

            Random random = new Random();
            long orderCode = (DateTime.Now.Ticks % 1000000000000000L) * 10 + random.Next(0, 1000);
            var transaction = new Transaction()
            {
                Id = Guid.NewGuid(),
                Amount = (decimal)premium.Price,
                OrderCode = orderCode,
                WalletId = wallet.Id,
                Type = TransactionTypeEnum.PREMIUM.GetDescriptionFromEnum(),
                Status = TransactionStatusEnum.SUCCESS.ToString(),
                IsActive = true,
                CreateAt = TimeUtil.GetCurrentSEATime(),
                UpdateAt = TimeUtil.GetCurrentSEATime(),
            };
            await _unitOfWork.GetRepository<Transaction>().InsertAsync(transaction);

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Mua gói premium thành công",
                    data = _mapper.Map<GetTransactionResponse>(transaction)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Mua gói premium thất bại",
                data = null
            };
        }

        public async Task<BaseResponse> CreatePremium(CreatePremiumRequest request)
        {
            var premium = _mapper.Map<CreatePremiumRequest, Premium>(request);   
            
            await _unitOfWork.GetRepository<Premium>().InsertAsync(premium);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Tạo gói premium thành công",
                    data = _mapper.Map<CreatePremiumResponse>(premium)
                };
            }
            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Tạo gói premium thất bại",
                data = null
            };
        }

        public async Task<BaseResponse> GetAllPremium(int page, int size, int? friend,
                                                                            bool? timelimit,
                                                                            int? match,
                                                                            double? minPrice,
                                                                            double? maxPrice,
                                                                            bool? sortByFriend,
                                                                            bool? sortByMatch,
                                                                            bool? sortByPrice,
                                                                            bool? sortByTimelimit)
        {
            if (page < 1 || size < 1)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Page hoặc size không hợp lệ.",
                    data = null
                };
            }

            var premium = await _unitOfWork.GetRepository<Premium>().GetPagingListAsync(
                selector: a => _mapper.Map<CreatePremiumResponse>(a),
                predicate: a => a.IsActive &&                        
                         (!friend.HasValue || a.Friend >= friend.Value) &&
                         (!timelimit.HasValue || a.Timelimit == timelimit.Value) &&
                         (!match.HasValue || a.Match >= match.Value) &&
                         (!minPrice.HasValue || a.Price >= minPrice.Value) &&
                         (!maxPrice.HasValue || a.Price <= maxPrice.Value),
                orderBy: l => sortByFriend.HasValue ? (sortByFriend.Value ? l.OrderBy(x => x.Friend) : l.OrderByDescending(x => x.Friend)) :
                              sortByPrice.HasValue ? (sortByPrice.Value ? l.OrderBy(x => x.Price) : l.OrderByDescending(x => x.Price)) :
                              sortByMatch.HasValue ? (sortByMatch.Value ? l.OrderBy(x => x.Match) : l.OrderByDescending(x => x.Match)) :
                              sortByTimelimit.HasValue ? (sortByTimelimit.Value ? l.OrderBy(x => x.Timelimit) : l.OrderByDescending(x => x.Timelimit)) :
                              l.OrderBy(x => x.CreateAt),
                page: page,
                size: size);

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy thông tin premium thành công",
                data = premium
            };
        }

        public async Task<BaseResponse> GetPremiumById(Guid premiumId)
        {
            var premium = await _unitOfWork.GetRepository<Premium>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(premiumId) && x.IsActive);

            if (premium == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin premium với ID này",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy thông tin premium thành công",
                data = _mapper.Map<CreatePremiumResponse>(premium)
            };
        }

        public async Task<BaseResponse> RemovePremium(Guid premiumId)
        {
            var premium = await _unitOfWork.GetRepository<Premium>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(premiumId) && x.IsActive);

            if (premium == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin premium với ID này",
                    data = null
                };
            }

            premium.IsActive = false;
            premium.DeleteAt = TimeUtil.GetCurrentSEATime(); 
            premium.UpdateAt = TimeUtil.GetCurrentSEATime(); 
            _unitOfWork.GetRepository<Premium>().UpdateAsync(premium);

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Xóa thông tin premium thành công",
                    data = isSuccessfully
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Xóa thông tin premium thất bại",
                data = isSuccessfully
            };
        }

        public async Task<BaseResponse> UpdatePremium(Guid premiumId, UpdatePremiumRequest request)
        {
            var premium = await _unitOfWork.GetRepository<Premium>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(premiumId) && x.IsActive);

            if (premium == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin premium với ID này",
                    data = null
                };
            }

            premium = _mapper.Map(request, premium);

            _unitOfWork.GetRepository<Premium>().UpdateAsync(premium);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Cập nhật thông tin premium thành công",
                    data = _mapper.Map<CreatePremiumResponse>(premium)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Cập nhật thông tin premium thất bại",
                data = null
            };
        }
    }
}
