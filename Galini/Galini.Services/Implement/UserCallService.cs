using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Enum;
using Galini.Models.Payload.Request.UserCall;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.UserCall;
using Galini.Models.Payload.Response.UserInfo;
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

namespace Galini.Services.Implement
{
    public class UserCallService : BaseService<UserCallService>, IUserCallService
    {
        private readonly IMapper _mapper;
        public UserCallService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<UserCallService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _mapper = mapper;
        }

        public async Task<BaseResponse> CreateUserCall(CreateUserCallRequest request, Guid accountId, Guid callHistoryId)
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

            var callHistory = await _unitOfWork.GetRepository<CallHistory>().SingleOrDefaultAsync(
                predicate: x => x.IsActive && x.Id.Equals(callHistoryId));

            if (callHistory == null)
            {
                _logger.LogWarning($"Không tìm thấy chi tiết lịch sử cuộc gọi có Id {callHistoryId} .");
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Chi tiết lịch sử cuộc gọi không tồn tại",
                    data = null
                };
            }

            var userCall = _mapper.Map<CreateUserCallRequest, UserCall>(request);
            userCall.AccountId = accountId;
            userCall.CallHistoryId = callHistoryId;

            await _unitOfWork.GetRepository<UserCall>().InsertAsync(userCall);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Tạo lịch sử cuộc gọi thành công",
                    data = _mapper.Map<CreateUserCallResponse>(userCall)
                };
            }
            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Tạo lịch sử cuộc gọi thất bại",
                data = null
            };
        }

        public async Task<BaseResponse> GetAllUserCall(int page, int size)
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

            var listUserCall = await _unitOfWork.GetRepository<UserCall>().GetPagingListAsync(
                selector: a => _mapper.Map<CreateUserCallResponse>(a),
                predicate: a => a.IsActive,
                page: page,
                size: size);

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy lịch sử cuộc gọi thành công",
                data = listUserCall
            };
        }

        public async Task<BaseResponse> GetUserCallByAccountId(int page, int size, Guid accountId)
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

            var userCall = await _unitOfWork.GetRepository<UserCall>().GetPagingListAsync(
                selector: a => _mapper.Map<CreateUserCallResponse>(a),
                predicate: x => x.AccountId.Equals(accountId) && x.IsActive,
                page: page,
                size: size);

            if (userCall == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy lịch sử cuộc gọi cho tài khoản này",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy lịch sử cuộc gọi thành công",
                data = _mapper.Map<CreateUserCallResponse>(userCall)
            };
        }

        public async Task<BaseResponse> GetUserCallById(Guid userCallId)
        {
            var userCall = await _unitOfWork.GetRepository<UserCall>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(userCallId) && x.IsActive);

            if (userCall == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy lịch sử cuộc gọi với ID này",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy lịch sử cuộc gọi thành công",
                data = _mapper.Map<CreateUserCallResponse>(userCall)
            };
        }

        public async Task<BaseResponse> RemoveUserCall(Guid userCallId)
        {
            var userCall = await _unitOfWork.GetRepository<UserCall>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(userCallId) && x.IsActive);

            if (userCall == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy lịch sử cuộc gọi với ID này",
                    data = null
                };
            }

            userCall.IsActive = false;
            userCall.DeleteAt = TimeUtil.GetCurrentSEATime();
            userCall.UpdateAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<UserCall>().UpdateAsync(userCall);

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Xóa lịch sử cuộc gọi thành công",
                    data = isSuccessfully
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Xóa lịch sử cuộc gọi thất bại",
                data = isSuccessfully
            };
        }

        public async Task<BaseResponse> UpdateUserCall(Guid id, UpdateUserCallRequest request, Guid accountId, Guid callHistoryId)
        {
            var userCall = await _unitOfWork.GetRepository<UserCall>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id) && x.IsActive);

            if (userCall == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy lịch sử cuộc gọi với ID này",
                    data = null
                };
            }

            userCall = _mapper.Map(request, userCall);
            userCall.AccountId = accountId;
            userCall.CallHistoryId = callHistoryId;

            _unitOfWork.GetRepository<UserCall>().UpdateAsync(userCall);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Cập nhật lịch sử cuộc gọi thành công",
                    data = _mapper.Map<CreateUserInfoResponse>(userCall)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Cập nhật lịch sử cuộc gọi thất bại",
                data = null
            };
        }
    }
}
