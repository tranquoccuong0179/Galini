using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Payload.Request.Notification;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.Notification;
using Galini.Repository.Interface;
using Galini.Services.Interface;
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

        public Task<BaseResponse> GetAllNotification(int page, int size)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> GetNotificationByAccountId(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> GetNotificationoById(Guid notificationId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> RemoveNotification(Guid notificationId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> UpdateNotification(Guid notificationId, CreateNotificationRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
