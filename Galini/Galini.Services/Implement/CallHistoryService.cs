using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Payload.Request.CallHistory;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.CallHistory;
using Galini.Models.Payload.Response.Premium;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Galini.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Implement
{
    public class CallHistoryService : BaseService<CallHistoryService>, ICallHistoryService
    {
        private readonly IMapper _mapper;
        public CallHistoryService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<CallHistoryService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _mapper = mapper;
        }

        public async Task<BaseResponse> CreateCallHistory(CreateCallHistoryRequest request)
        {
            var callHistory = _mapper.Map<CreateCallHistoryRequest, CallHistory>(request);
            await _unitOfWork.GetRepository<CallHistory>().InsertAsync(callHistory);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Tạo chi tiết lịch sử cuộc gọi thành công",
                    data = _mapper.Map<CreateCallHistoryResponse>(callHistory)
                };
            }
            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Tạo chi tiết lịch sử cuộc gọi thất bại",
                data = null
            };
        }

        public async Task<BaseResponse> GetAllCallHistory(int page, int size)
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

            var callHistory = await _unitOfWork.GetRepository<CallHistory>().GetPagingListAsync(
                selector: a => _mapper.Map<CreateCallHistoryResponse>(a),
                predicate: a => a.IsActive,
                page: page,
                size: size);

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy thông tin chi tiết lịch sử cuộc gọi thành công",
                data = callHistory
            };
        }

        public async Task<BaseResponse> GetCallHistoryById(Guid callHistoryId)
        {

            var callHistory = await _unitOfWork.GetRepository<CallHistory>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(callHistoryId) && x.IsActive);

            if (callHistory == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin chi tiết lịch sử cuộc gọi này",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy thông tin chi tiết lịch sử cuộc gọi thành công",
                data = _mapper.Map<CreateCallHistoryResponse>(callHistory)
            };
        }

        public async Task<BaseResponse> RemoveCallHistory(Guid callHistoryId)
        {
            var callHistory = await _unitOfWork.GetRepository<CallHistory>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(callHistoryId) && x.IsActive);

            if (callHistory == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin chi tiết lịch sử cuộc gọi với ID này",
                    data = null
                };
            }

            callHistory.IsActive = false;
            callHistory.DeleteAt = TimeUtil.GetCurrentSEATime();
            callHistory.UpdateAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<CallHistory>().UpdateAsync(callHistory);

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Xóa thông tin chi tiết lịch sử cuộc gọi thành công",
                    data = isSuccessfully
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Xóa thông tin chi tiết lịch sử cuộc gọi thất bại",
                data = isSuccessfully
            };
        }

        public async Task<BaseResponse> UpdateCallHistory(Guid callHistoryId, UpdateCallHistoryRequest request)
        {
            var callHistory = await _unitOfWork.GetRepository<CallHistory>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(callHistoryId) && x.IsActive);

            if (callHistory == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin chi tiết lịch sử cuộc gọi với ID này",
                    data = null
                };
            }

            callHistory = _mapper.Map(request, callHistory);

            _unitOfWork.GetRepository<CallHistory>().UpdateAsync(callHistory);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Cập nhật thông tin chi tiết lịch sử cuộc gọi thành công",
                    data = _mapper.Map<CreateCallHistoryResponse>(callHistory)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Cập nhật thông tin chi tiết lịch sử cuộc gọi thất bại",
                data = null
            };
        }
    }
}
