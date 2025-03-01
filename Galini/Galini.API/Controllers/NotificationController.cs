
using Galini.API.Constants;
using Galini.API.Infrastructure;
using Galini.Models.Enum;
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

        /// <summary>
        /// API tạo thông báo mới.
        /// </summary>
        /// <remarks>
        /// - Tạo một thông báo mới cho người dùng dựa trên `userId`.  
        /// - Yêu cầu dữ liệu hợp lệ trong `request`.  
        /// - Nếu `userId` không tồn tại, trả về lỗi `404 Not Found`.  
        /// - Nếu dữ liệu không hợp lệ, trả về lỗi `400 Bad Request`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="request">Thông tin của thông báo cần tạo.</param>
        /// <param name="userId">ID của người dùng nhận thông báo.</param>
        /// <returns>
        /// - `200 OK`: Tạo thông báo thành công.  
        /// - `400 Bad Request`: Dữ liệu không hợp lệ.  
        /// - `404 Not Found`: Không tìm thấy người dùng.
        /// </returns>
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

        /// <summary>
        /// API lấy danh sách thông báo với phân trang, hỗ trợ lọc theo loại thông báo và khoảng thời gian.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách thông báo có hỗ trợ phân trang.  
        /// - Nếu không truyền `page` hoặc `size`, giá trị mặc định sẽ được sử dụng (`page = 1`, `size = 10`).  
        /// - Có thể lọc theo loại thông báo (`type`) và khoảng thời gian (`daysAgo`, `weeksAgo`, `monthsAgo`).  
        /// - Khi không có bất kỳ bộ lọc nào, danh sách sẽ được sắp xếp theo thời gian tạo giảm dần.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="page">Số trang hiện tại (mặc định là 1).</param>
        /// <param name="size">Số lượng thông báo trên mỗi trang (mặc định là 10).</param>
        /// <param name="type">Lọc danh sách theo loại thông báo.</param>
        /// <param name="daysAgo">Lọc thông báo trong số ngày trước.</param>
        /// <param name="weeksAgo">Lọc thông báo trong số tuần trước.</param>
        /// <param name="monthsAgo">Lọc thông báo trong số tháng trước.</param>
        /// <returns>
        /// - `200 OK`: Trả về danh sách thông báo thành công.  
        /// </returns>
        [CustomAuthorize(roles: "Customer")]
        [HttpGet(ApiEndPointConstant.Notification.GetNotifications)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetNotifications([FromQuery] int? page,
                                                          [FromQuery] int? size,
                                                          [FromQuery] TypeEnum? type,
                                                          [FromQuery] int? daysAgo,
                                                          [FromQuery] int? weeksAgo,
                                                          [FromQuery] int? monthsAgo)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _notificationService.GetAllNotification(pageNumber, pageSize, type, daysAgo, weeksAgo, monthsAgo);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy chi tiết một thông báo.
        /// </summary>
        /// <remarks>
        /// - Lấy thông tin chi tiết của một thông báo của người dùng đó dựa trên `id`.  
        /// - Nếu không tìm thấy thông báo, trả về lỗi `404 Not Found`.  
        /// - Chỉ người dùng có quyền "Customer" mới được truy cập.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của thông báo cần lấy.</param>
        /// <returns>
        /// - `200 OK`: Lấy thông báo thành công.  
        /// - `404 Not Found`: Không tìm thấy thông báo.
        /// </returns>
        [CustomAuthorize(roles: "Customer")]
        [HttpGet(ApiEndPointConstant.Notification.GetNotification)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetNotification([FromRoute] Guid id)
        {
            var response = await _notificationService.GetNotificationoById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API xóa một thông báo.
        /// </summary>
        /// <remarks>
        /// - Xóa thông báo của người dùng đó dựa trên `id` được cung cấp.  
        /// - Nếu không tìm thấy thông báo, trả về lỗi `404 Not Found`.  
        /// - Nếu không thể xóa do dữ liệu không hợp lệ, trả về lỗi `400 Bad Request`.  
        /// - Chỉ người dùng có quyền "Customer" mới được truy cập.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của thông báo cần xóa.</param>
        /// <returns>
        /// - `200 OK`: Xóa thông báo thành công.  
        /// - `400 Bad Request`: Không thể xóa thông báo.  
        /// - `404 Not Found`: Không tìm thấy thông báo.
        /// </returns>
        [CustomAuthorize(roles: "Customer")]
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

        /// <summary>
        /// API đánh dấu thông báo là đã đọc.
        /// </summary>
        /// <remarks>
        /// - Cập nhật trạng thái của thông báo thành "đã đọc" dựa trên `id`.  
        /// - Nếu không tìm thấy thông báo, trả về lỗi `404 Not Found`.  
        /// - Chỉ người dùng có quyền "Customer" mới được truy cập.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của thông báo cần đánh dấu là đã đọc.</param>
        /// <returns>
        /// - `200 OK`: Cập nhật trạng thái thành công.  
        /// - `404 Not Found`: Không tìm thấy thông báo.
        /// </returns>
        [CustomAuthorize(roles: "Customer")]
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
