
using Galini.API.Constants;
using Galini.Models.Payload.Request.Notification;
using Galini.Models.Payload.Request.User;
using Galini.Models.Payload.Response;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class NotificationController : BaseController<NotificationController>
    {
        private readonly INotificationService _notificationService;
        public NotificationController(ILogger<NotificationController> logger, INotificationService notificationService) : base(logger)
        {
            _notificationService = notificationService;
        }

        [HttpPost(ApiEndPointConstant.Notification.CreateNotification)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationRequest request, [FromQuery] Guid userId)
        {
            var response = await _notificationService.CreateNotification(request, userId);
            return StatusCode(int.Parse(response.status), response);
        }
        
        [HttpGet(ApiEndPointConstant.Notification.GetNotifications)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetNotifications([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _notificationService.GetAllNotification(pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }
        
        [HttpGet(ApiEndPointConstant.Notification.GetNotification)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateNotification([FromRoute] Guid id)
        {
            var response = await _notificationService.GetNotificationoById(id);
            return StatusCode(int.Parse(response.status), response);
        }
        
        [HttpDelete(ApiEndPointConstant.Notification.RemoveNotification)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RemoveNotification([FromRoute] Guid id)
        {
            var response = await _notificationService.RemoveNotification(id);
            return StatusCode(int.Parse(response.status), response);
        }
        
        [HttpPut(ApiEndPointConstant.Notification.MarkNotificationAsRead)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> MarkNotificationAsRead([FromRoute] Guid id)
        {
            var response = await _notificationService.MarkNotificationAsRead(id);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}
