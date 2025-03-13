using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Payload.Request.UserCall;
using Galini.Models.Payload.Request.UserInfo;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.UserInfo;
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
    public class UserInfoService : BaseService<UserInfoService>, IUserInfoService
    {
        private readonly IMapper _mapper;
        public UserInfoService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<UserInfoService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _mapper = mapper;
        }

        public async Task<BaseResponse> CreateUserInfo(Guid accountId, Guid premiumId)
        {
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.IsActive && x.Id.Equals(accountId));

            if (account == null)
            {
                _logger.LogWarning($"Không tìm thấy tài khoản có Id {accountId} .");
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Tài khoản không tồn tại",
                    data = null
                };
            }

            var premium = await _unitOfWork.GetRepository<Premium>().SingleOrDefaultAsync(
                predicate: x => x.IsActive && x.Id.Equals(premiumId));

            if (premium == null)
            {
                _logger.LogWarning($"Không tìm thấy premium có Id {premiumId} .");
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Premium không tồn tại",
                    data = null
                };
            }

            var userInfor = new UserInfo();
            userInfor.Id = Guid.NewGuid();
            userInfor.PremiumId = premiumId;
            userInfor.AccountId = accountId;
            userInfor.DateStart = TimeUtil.GetCurrentSEATime();
            userInfor.DateEnd = TimeUtil.GetCurrentSEATime().AddDays(30);
            userInfor.IsActive = true;
            userInfor.CreateAt = TimeUtil.GetCurrentSEATime(); 
            userInfor.UpdateAt = TimeUtil.GetCurrentSEATime();

            await _unitOfWork.GetRepository<UserInfo>().InsertAsync(userInfor);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Tạo thông tin người dùng thành công",
                    data = _mapper.Map<CreateUserInfoResponse>(userInfor)
                };
            }
            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Tạo thông tin người dùng thất bại",
                data = null
            };
        }

        public async Task<BaseResponse> GetAllUserInfo(int page, int size, string? premium, bool? sortByPremium)
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

            var listUserInfo = await _unitOfWork.GetRepository<UserInfo>().GetPagingListAsync(
                selector: a => _mapper.Map<CreateUserInfoResponse>(a),
                predicate: a => a.IsActive && (string.IsNullOrEmpty(premium) || a.Premium.Type.Equals(premium)),
                orderBy: l => sortByPremium.HasValue ? (sortByPremium.Value ? l.OrderBy(l => l.Premium.Type) : l.OrderByDescending(l => l.Premium.Type)) : l.OrderBy(l => l.CreateAt),
                include: l => l.Include(l => l.Premium).Include(l => l.Account),
                page: page,
                size: size);

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy thông tin người dùng thành công",
                data = listUserInfo
            };
        }

        public async Task<BaseResponse> GetUserInfoByAccountId(Guid accountId)
        {
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.IsActive && x.Id.Equals(accountId));

            if (account == null)
            {
                _logger.LogWarning($"Không tìm thấy tài khoản có Id {accountId} .");
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Tài khoản không tồn tại",
                    data = null
                };
            }

            var userInfo = await _unitOfWork.GetRepository<UserInfo>().SingleOrDefaultAsync(
                predicate: x => x.AccountId.Equals(accountId) && x.IsActive,
                include: l => l.Include(l => l.Premium).Include(l => l.Account));


            if (userInfo == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin người dùng cho tài khoản này",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy thông tin người dùng thành công",
                data = _mapper.Map<CreateUserInfoResponse>(userInfo)
            };
        }

        public async Task<BaseResponse> GetUserInfoById(Guid userInfoId)
        {
            var userInfo = await _unitOfWork.GetRepository<UserInfo>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(userInfoId) && x.IsActive,
                include: l => l.Include(l => l.Premium).Include(l => l.Account));

            if (userInfo == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin người dùng với ID này",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy thông tin người dùng thành công",
                data = _mapper.Map<CreateUserInfoResponse>(userInfo)
            };
        }

        public async Task<BaseResponse> RemoveUserInfo(Guid userInfoId)
        {
            var userInfo = await _unitOfWork.GetRepository<UserInfo>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(userInfoId) && x.IsActive);

            if (userInfo == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin người dùng với ID này",
                    data = null
                };
            }

            userInfo.IsActive = false;
            userInfo.DeleteAt = TimeUtil.GetCurrentSEATime();
            userInfo.UpdateAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<UserInfo>().UpdateAsync(userInfo);

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Xóa thông tin người dùng thành công",
                    data = isSuccessfully
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Xóa thông tin người dùng thất bại",
                data = isSuccessfully
            };
        }

        public async Task<BaseResponse> UpdateUserInfo(Guid userInfoId, Guid premiumId, UpdateUserInfoRequest request)
        {
            var userInfo = await _unitOfWork.GetRepository<UserInfo>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(userInfoId) && x.IsActive);

            if (userInfo == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin người dùng với ID này",
                    data = null
                };
            }

            userInfo = _mapper.Map(request, userInfo);
            userInfo.PremiumId = premiumId;

            _unitOfWork.GetRepository<UserInfo>().UpdateAsync(userInfo);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Cập nhật thông tin người dùng thành công",
                    data = _mapper.Map<CreateUserInfoResponse>(userInfo)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Cập nhật thông tin người dùng thất bại",
                data = null
            };
        }
    }
}
