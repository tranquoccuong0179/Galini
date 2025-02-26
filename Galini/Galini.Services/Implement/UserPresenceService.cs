using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Payload.Request.UserPresence;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.UserPresence;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Galini.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Galini.Services.Implement
{
    public class UserPresenceService : BaseService<UserPresenceService>, IUserPresenceService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public UserPresenceService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<UserPresenceService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse> CreateUserPresence(CreateUserPresenceRequest request, Guid id)
        {
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
               predicate: l => l.Id.Equals(id) && l.IsActive);

            if (account == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy người dùng",
                    data = null
                };
            }

            var userPresence = _mapper.Map<CreateUserPresenceRequest, UserPresence>(request);
            userPresence.AccountId = account.Id;

            await _unitOfWork.GetRepository<UserPresence>().InsertAsync(userPresence);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Thêm user presence thành công",
                    data = userPresence
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Thêm user presence thất bại",
                data = null
            };
        }

        public async Task<BaseResponse> GetAllUserPresence(int page, int size)
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

            var userPresence = await _unitOfWork.GetRepository<UserPresence>().GetPagingListAsync(
                selector: a => _mapper.Map<CreateUserPresenceResponse>(a),
                predicate: a => a.IsActive,
                orderBy: a => a.OrderBy(a => a.CreateAt),
                page: page,
                size: size);

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy user presence thành công",
                data = userPresence
            };
        }

        public async Task<BaseResponse> GetUserPresenceByAccountId(Guid id)
        {
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(id) && a.IsActive);

            if (account == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy người dùng",
                    data = null
                };
            }

            var userPresence = await _unitOfWork.GetRepository<UserPresence>().SingleOrDefaultAsync(
                selector: a => _mapper.Map<CreateUserPresenceResponse>(a),
                predicate: a => a.AccountId.Equals(id) && a.IsActive);

            if (userPresence == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy user presence",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy user presence thành công",
                data = userPresence
            };
        }

        public async Task<BaseResponse> GetUserPresenceById(Guid id)
        {
            var userPresence = await _unitOfWork.GetRepository<UserPresence>().SingleOrDefaultAsync(
                selector: a => _mapper.Map<CreateUserPresenceResponse>(a),
                predicate: a => a.Id.Equals(id) && a.IsActive);

            if (userPresence == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy user presence",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy user presence thành công",
                data = userPresence
            };
        }

        public async Task<BaseResponse> RemoveUserPresence(Guid id)
        {
            var userPresence = await _unitOfWork.GetRepository<UserPresence>().SingleOrDefaultAsync(
                predicate: q => q.IsActive && q.Id.Equals(id));

            if (userPresence == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy user presence",
                    data = false
                };
            }

            userPresence.IsActive = false;
            userPresence.DeleteAt = TimeUtil.GetCurrentSEATime();
            userPresence.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<UserPresence>().UpdateAsync(userPresence);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Xóa user presence thành công",
                    data = true
                };
            }
            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Xóa user presence thất bại",
                data = false
            };
        }

        public async Task<BaseResponse> UpdateUserPresence(Guid id, UpdateUserPresenceRequest request)
        {
            var userPresence = await _unitOfWork.GetRepository<UserPresence>().SingleOrDefaultAsync(
                predicate: t => t.Id.Equals(id) && t.IsActive
                );

            if (userPresence == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy user presence này",
                    data = null
                };
            }

            _mapper.Map(request, userPresence);
            _unitOfWork.GetRepository<UserPresence>().UpdateAsync(userPresence);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Cập nhật user presence thành công",
                    data = _mapper.Map<CreateUserPresenceResponse>(userPresence)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Cập nhật user presence thất bại",
                data = null
            };
        }
    }
}
