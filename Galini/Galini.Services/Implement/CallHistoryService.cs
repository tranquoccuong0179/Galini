﻿using AutoMapper;
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

        public async Task<BaseResponse> GetAllCallHistory(int page, int size, DateTime? timeStart, DateTime? timeEnd, int? duration, bool? isMissCall, bool? sortByTimeStart, bool? sortByTimeEnd, bool? sortByDuration, bool? sortByMissCall)
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
                predicate: a => a.IsActive &&
                                (!timeStart.HasValue || a.TimeStart.Date == timeStart.Value.Date) &&
                                (!timeEnd.HasValue || a.TimeStart.Date == timeEnd.Value.Date) &&
                                (!isMissCall.HasValue || a.IsMissCall == isMissCall),
                orderBy: l => sortByMissCall.HasValue ? (sortByMissCall.Value ? l.OrderBy(l => l.IsMissCall) : l.OrderByDescending(l => l.IsMissCall)) :
                              sortByTimeStart.HasValue ? (sortByTimeStart.Value ? l.OrderBy(l => l.TimeStart) : l.OrderByDescending(l => l.TimeStart)) :
                              sortByTimeEnd.HasValue ? (sortByTimeEnd.Value ? l.OrderBy(l => l.TimeEnd) : l.OrderByDescending(l => l.TimeEnd)) :
                              sortByDuration.HasValue ? (sortByDuration.Value ? l.OrderBy(l => l.Duration) : l.OrderByDescending(l => l.Duration)) :
                              l.OrderByDescending(l => l.CreateAt),
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

            var userCalls = await _unitOfWork.GetRepository<UserCall>().GetListAsync(
                predicate: x => x.CallHistoryId.Equals(callHistory.Id) && x.IsActive);

            if (userCalls != null)
            {
                foreach (var userCall in userCalls)
                {
                    userCall.IsActive = false;
                    userCall.UpdateAt = TimeUtil.GetCurrentSEATime();
                    userCall.DeleteAt = TimeUtil.GetCurrentSEATime();
                    _unitOfWork.GetRepository<UserCall>().UpdateAsync(userCall);
                }
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
