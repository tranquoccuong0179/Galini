using Galini.Models.Enum;
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
        public Task<BaseResponse> GetAllNotification(int page, int size, TypeEnum? type, int? daysAgo, int? monthsAgo);
        public Task<BaseResponse> GetNotificationoById(Guid id);
        public Task<BaseResponse> MarkNotificationAsRead(Guid id);
        public Task<BaseResponse> RemoveNotification(Guid id);
    }
}
