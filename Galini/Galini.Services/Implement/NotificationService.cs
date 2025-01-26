using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Paginate;
using Galini.Models.Payload.Request.Notification;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.Notification;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Galini.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Galini.Services.Implement
{
    public class NotificationService : BaseService<NotificationService>, INotificationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public NotificationService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<NotificationService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<BaseResponse> CreateNotification(CreateNotificationRequest request, Guid userId)
        {
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.IsActive == true && a.Id.Equals(userId));
            if (account == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Tài khoản không tồn tại",
                    data = null
                };
            }
            var notification = _mapper.Map<Notification>(request);
            notification.UserId = userId;
            await _unitOfWork.GetRepository<Notification>().InsertAsync(notification);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;
            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Tạo thông báo thành công",
                    data = _mapper.Map<CreateNotificationResponse>(notification)
                };
            }
            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Tạo thông báo thất bại",
                data = null
            };
        }

        public async Task<BaseResponse> GetAllNotification(int page, int size)
        {
            var response = await _unitOfWork.GetRepository<Notification>().GetPagingListAsync(
                selector: n => _mapper.Map<GetNotificationResponse>(n),
                predicate: n => n.IsActive == true,
                page: page,
                size: size);

            int totalItems = response.Total;
            int totalPages = (int)Math.Ceiling((double)totalItems / size);

            if (response == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Danh sách thông báo",
                    data = new Paginate<Notification>()
                    {
                        Page = page,
                        Size = size,
                        Total = totalItems,
                        TotalPages = totalPages,
                        Items = new List<Notification>()
                    }
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Danh sách thông báo",
                data = response
            };
        }

        public async Task<BaseResponse> GetNotificationoById(Guid id)
        {
            var response = await _unitOfWork.GetRepository<Notification>().SingleOrDefaultAsync(
                selector: n => _mapper.Map<GetNotificationResponse>(n),
                predicate: n => n.IsActive == true && n.Id.Equals(id));
            if (response == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông báo",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Thông báo",
                data = response
            };
        }

        public async Task<BaseResponse> RemoveNotification(Guid id)
        {
            var notification = await _unitOfWork.GetRepository<Notification>().SingleOrDefaultAsync(
                predicate: n => n.IsActive == true && n.Id.Equals(id));

            if(notification == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông báo",
                    data = false
                };
            }

            notification.IsActive = false;
            notification.DeleteAt = TimeUtil.GetCurrentSEATime();
            notification.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<Notification>().UpdateAsync(notification);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Xóa thông báo thành công",
                    data = true
                };
            }
            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Xóa thông báo thất bại",
                data = false
            };
        }

        public async Task<BaseResponse> MarkNotificationAsRead(Guid id)
        {
            var notification = await _unitOfWork.GetRepository<Notification>().SingleOrDefaultAsync(
                predicate: n => n.IsActive == true && n.Id.Equals(id));

            if(notification == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông báo",
                    data = false
                };
            }

            notification.IsRead = true;
            notification.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<Notification>().UpdateAsync(notification);
            await _unitOfWork.CommitAsync();

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Đọc thông báo thành công",
                data = true
            };

        }
    }
}
