using Galini.API.Constants;
using Galini.Models.Enum;
using Galini.Models.Payload.Request.Booking;
using Galini.Models.Payload.Request.FriendShip;
using Galini.Models.Payload.Response;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class BookingController : BaseController<BookingController>
    {
        private readonly IBookingService _bookingService;
        public BookingController(ILogger<BookingController> logger, IBookingService bookingService) : base(logger)
        {
            _bookingService = bookingService;
        }

        /// <summary>
        /// API tạo đặt lịch mới.
        /// </summary>
        /// <remarks>
        /// - Tạo đặt lịch dựa trên `request` và `id` ca làm việc.  
        /// - Kiểm tra tài khoản người dùng, ca làm việc và tham vấn viên có tồn tại không, nếu không trả `404`.  
        /// - Nếu lưu thất bại, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="request">Dữ liệu đặt lịch cần tạo.</param>
        /// <param name="id">ID của ca làm việc.</param>
        /// <returns>
        /// - `200 OK`: Tạo đặt lịch thành công.  
        /// - `400 Bad Request`: Lưu thất bại.  
        /// - `404 Not Found`: Không tìm thấy tài khoản, ca làm việc hoặc tham vấn viên.
        /// </returns>
        [HttpPost(ApiEndPointConstant.Booking.CreateBooking)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request, [FromQuery] Guid id)
        {
            var response = await _bookingService.CreateBooking(request, id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách tất cả đặt lịch với phân trang.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách đặt lịch còn hoạt động, mặc định `page = 1`, `size = 10`.  
        /// - Lọc theo trạng thái (`bookingEnum`) nếu có, sắp xếp giảm dần theo thời gian tạo.  
        /// - Nếu `page` hoặc `size` nhỏ hơn 1, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="page">Số trang (mặc định 1).</param>
        /// <param name="size">Số lượng mỗi trang (mặc định 10).</param>
        /// <param name="bookingEnum">Lọc theo trạng thái đặt lịch.</param>
        /// <returns>
        /// - `200 OK`: Lấy danh sách đặt lịch thành công.  
        /// - `400 Bad Request`: Page hoặc size không hợp lệ.
        /// </returns>
        [HttpGet(ApiEndPointConstant.Booking.GetAllBookings)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllBookings([FromQuery] int? page, [FromQuery] int? size, [FromQuery] BookingEnum? bookingEnum)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _bookingService.GetAllBookings(pageNumber, pageSize, bookingEnum);

            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách đặt lịch của người dùng với phân trang.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách đặt lịch liên quan đến người dùng (làm user hoặc listener), mặc định `page = 1`, `size = 10`.  
        /// - Lọc theo trạng thái (`bookingEnum`) nếu có, sắp xếp giảm dần theo thời gian tạo.  
        /// - Nếu tài khoản không tồn tại, trả `404`. Nếu `page` hoặc `size` nhỏ hơn 1, trả `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="page">Số trang (mặc định 1).</param>
        /// <param name="size">Số lượng mỗi trang (mặc định 10).</param>
        /// <param name="bookingEnum">Lọc theo trạng thái đặt lịch.</param>
        /// <returns>
        /// - `200 OK`: Lấy danh sách đặt lịch thành công.  
        /// - `400 Bad Request`: Page hoặc size không hợp lệ.  
        /// - `404 Not Found`: Không tìm thấy tài khoản người dùng.
        /// </returns>
        [HttpGet(ApiEndPointConstant.Booking.GetUserBookings)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetUserBookings([FromQuery] int? page, [FromQuery] int? size, [FromQuery] BookingEnum? bookingEnum)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _bookingService.GetUserBookings(pageNumber, pageSize, bookingEnum);    

            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy thông tin đặt lịch theo ID.
        /// </summary>
        /// <remarks>
        /// - Trả về chi tiết đặt lịch dựa trên `id`.  
        /// - Chỉ lấy nếu còn hoạt động (`IsActive = true`).  
        /// - Nếu không tìm thấy, trả lỗi `404`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của đặt lịch.</param>
        /// <returns>
        /// - `200 OK`: Lấy đặt lịch thành công.  
        /// - `404 Not Found`: Không tìm thấy đặt lịch.
        /// </returns>
        [HttpGet(ApiEndPointConstant.Booking.GetBookingById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetBookingById([FromRoute] Guid id)
        {
            var response = await _bookingService.GetBookingById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API cập nhật thông tin đặt lịch theo ID.
        /// </summary>
        /// <remarks>
        /// - Cập nhật đặt lịch dựa trên `id` và `request`.  
        /// - Nếu `id` không tồn tại hoặc không hoạt động, trả lỗi `404`.  
        /// - Nếu cập nhật thất bại, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của đặt lịch cần cập nhật.</param>
        /// <param name="request">Thông tin cập nhật cho đặt lịch.</param>
        /// <returns>
        /// - `200 OK`: Cập nhật đặt lịch thành công.  
        /// - `400 Bad Request`: Cập nhật thất bại.  
        /// - `404 Not Found`: Không tìm thấy đặt lịch.
        /// </returns>
        [HttpPatch(ApiEndPointConstant.Booking.UpdateBooking)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateBooking([FromRoute] Guid id, [FromBody] UpdateBookingRequest request)
        {

            var response = await _bookingService.UpdateBooking(id, request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API xoá đặt lịch theo ID.
        /// </summary>
        /// <remarks>
        /// - Xoá đặt lịch dựa trên `id` (chuyển `IsActive = false`).  
        /// - Nếu không tìm thấy đặt lịch, trả lỗi `404`.  
        /// - Nếu không xoá được, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của đặt lịch cần xoá.</param>
        /// <returns>
        /// - `200 OK`: Xoá đặt lịch thành công.  
        /// - `400 Bad Request`: Không thể xoá đặt lịch.  
        /// - `404 Not Found`: Không tìm thấy đặt lịch.
        /// </returns>
        [HttpDelete(ApiEndPointConstant.Booking.RemoveBooking)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RemoveBooking([FromRoute] Guid id)
        {
            var response = await _bookingService.RemoveBooking(id);

            return StatusCode(int.Parse(response.status), response);
        }
    }
}
