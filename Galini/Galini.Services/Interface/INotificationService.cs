using Galini.Models.Payload.Request.Notification;
using Galini.Models.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface INotificationService
    {
        public Task<BaseResponse> CreateNotification(CreateNotificationRequest request, Guid userId);
        public Task<BaseResponse> GetAllNotification(int page, int size);
        public Task<BaseResponse> GetNotificationoById(Guid id);
        public Task<BaseResponse> UpdateNotification(Guid notificationId, CreateNotificationRequest request);
        public Task<BaseResponse> RemoveNotification(Guid id);
    }
}
