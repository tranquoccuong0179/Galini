using AutoMapper;
using Azure;
using Galini.Models.Entity;
using Galini.Models.Payload.Request.WorkShift;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.UserPresence;
using Galini.Models.Payload.Response.WorkShift;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Galini.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Implement
{
    public class WorkShiftService : BaseService<WorkShiftService>, IWorkShiftService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public WorkShiftService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<WorkShiftService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse> CreateWorkShift(CreateWorkShiftRequest request, Guid accountId)
        {
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
               predicate: l => l.Id.Equals(accountId) && l.IsActive);

            if (account == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy người dùng",
                    data = null
                };
            }

            var workShift = _mapper.Map<CreateWorkShiftRequest, WorkShift>(request);
            workShift.AccountId = account.Id;

            await _unitOfWork.GetRepository<WorkShift>().InsertAsync(workShift);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Thêm ca làm việc thành công",
                    data = workShift
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Thêm ca làm việc thất bại",
                data = null
            };
        }

        public async Task<BaseResponse> GetAllWorkShift(int page, int size)
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

            var workShift = await _unitOfWork.GetRepository<WorkShift>().GetPagingListAsync(
                selector: a => _mapper.Map<CreateWorkShiftResponse>(a),
                predicate: a => a.IsActive,
                orderBy: a => a.OrderBy(a => a.CreateAt),
                page: page,
                size: size);

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy ca làm việc thành công",
                data = workShift
            };
        }

        public async Task<BaseResponse> GetWorkShiftByAccountId(int page, int size, Guid id)
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

            var workShift = await _unitOfWork.GetRepository<WorkShift>().GetPagingListAsync(
                selector: a => _mapper.Map<CreateWorkShiftResponse>(a),
                predicate: a => a.IsActive && a.AccountId.Equals(id),
                orderBy: a => a.OrderBy(a => a.CreateAt),
                page: page,
                size: size);

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy ca làm việc thành công",
                data = workShift
            };
        }

        public async Task<BaseResponse> GetWorkShiftById(Guid id)
        {
            var workShift = await _unitOfWork.GetRepository<WorkShift>().SingleOrDefaultAsync(
                selector: a => _mapper.Map<CreateWorkShiftResponse>(a),
                predicate: a => a.Id.Equals(id) && a.IsActive);

            if (workShift == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy ca làm việc",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy ca làm việc thành công",
                data = workShift
            };
        }

        public async Task<BaseResponse> RemoveWorkShift(Guid id)
        {
            var workShift = await _unitOfWork.GetRepository<WorkShift>().SingleOrDefaultAsync(
                predicate: q => q.IsActive && q.Id.Equals(id));

            if (workShift == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy ca làm việc",
                    data = false
                };
            }

            workShift.IsActive = false;
            workShift.DeleteAt = TimeUtil.GetCurrentSEATime();
            workShift.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<WorkShift>().UpdateAsync(workShift);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Xóa ca làm việc thành công",
                    data = true
                };
            }
            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Xóa ca làm việc thất bại",
                data = false
            };
        }

        public async Task<BaseResponse> UpdateWorkShift(UpdateWorkShiftRequest request, Guid id)
        {
            var workShift = await _unitOfWork.GetRepository<WorkShift>().SingleOrDefaultAsync(
                predicate: t => t.Id.Equals(id) && t.IsActive
                );

            if (workShift == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy ca làm việc này",
                    data = null
                };
            }

            _mapper.Map(request, workShift);
            _unitOfWork.GetRepository<WorkShift>().UpdateAsync(workShift);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Cập nhật ca làm việc thành công",
                    data = _mapper.Map<CreateWorkShiftResponse>(workShift)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Cập nhật ca làm việc thất bại",
                data = null
            };
        }
    }
}
