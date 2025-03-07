using Galini.API.Constants;
using Galini.Models.Enum;
using Galini.Models.Payload.Request.WorkShift;
using Galini.Models.Payload.Response;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class WorkShiftController : BaseController<WorkShiftController>
    {
        private readonly IWorkShiftService _workShiftService;
        public WorkShiftController(ILogger<WorkShiftController> logger, IWorkShiftService workShiftService) : base(logger)
        {
            _workShiftService = workShiftService;
        }

    /// <summary>
    /// API tạo ca làm việc mới.
    /// </summary>
    /// <remarks>
    /// - Tạo ca làm việc dựa trên `request` và `id` tài khoản.  
    /// - Kiểm tra tài khoản có tồn tại không, nếu không trả `404`.  
    /// - Nếu dữ liệu `request` không hợp lệ, trả lỗi `400`.  
    /// - Kết quả bọc trong `BaseResponse`.
    /// - Request cần tạo có form như sau: {  "startTime": "00:00:00",  "endTime": "00:00:00"}
    /// </remarks>
    /// <param name="request">Dữ liệu ca làm việc cần tạo.</param>
    /// <param name="id">ID của tài khoản người dùng.</param>
    /// <returns>
    /// - `200 OK`: Tạo ca làm việc thành công.  
    /// - `400 Bad Request`: Dữ liệu không hợp lệ hoặc lưu thất bại.  
    /// - `404 Not Found`: Không tìm thấy tài khoản.
    /// </returns>
    [HttpPost(ApiEndPointConstant.WorkShift.CreateWorkShift)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateWorkShift([FromBody] CreateWorkShiftRequest request, [FromRoute] Guid id, [FromQuery] DayEnum day)
        {
            if (request == null)
            {
                return BadRequest(new BaseResponse
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Invalid request data",
                    data = null
                });
            }

            var response = await _workShiftService.CreateWorkShift(request, id, day);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách ca làm việc với phân trang.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách ca làm việc còn hoạt động, mặc định `page = 1`, `size = 10`.  
        /// - Sắp xếp theo thời gian tạo tăng dần, không hỗ trợ lọc hay sắp xếp tùy chỉnh.  
        /// - Nếu `page` hoặc `size` nhỏ hơn 1, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="page">Số trang (mặc định 1).</param>
        /// <param name="size">Số lượng mỗi trang (mặc định 10).</param>
        /// <returns>
        /// - `200 OK`: Lấy danh sách ca làm việc thành công.  
        /// - `400 Bad Request`: Page hoặc size không hợp lệ.
        /// </returns>
        [HttpGet(ApiEndPointConstant.WorkShift.GetAllWorkShift)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllWorkShift([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _workShiftService.GetAllWorkShift(pageNumber, pageSize);

            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy ca làm việc theo ID.
        /// </summary>
        /// <remarks>
        /// - Trả về chi tiết ca làm việc dựa trên `id`.  
        /// - Chỉ lấy nếu còn hoạt động (`IsActive = true`).  
        /// - Nếu không tìm thấy, trả lỗi `404`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của ca làm việc.</param>
        /// <returns>
        /// - `200 OK`: Lấy ca làm việc thành công.  
        /// - `404 Not Found`: Không tìm thấy ca làm việc.
        /// </returns>
        [HttpGet(ApiEndPointConstant.WorkShift.GetWorkShiftById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetWorkShiftById([FromRoute] Guid id)
        {
            var response = await _workShiftService.GetWorkShiftById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// - API lấy danh sách ca làm việc theo ID tài khoản với phân trang.
        /// - Sử dụng để lấy danh sách các ca làm việc của từng tham vấn viên cho người dùng lựa chọn.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách ca làm việc của tài khoản, mặc định `page = 1`, `size = 10`.  
        /// - Chỉ lấy ca còn hoạt động (`IsActive = true`), sắp xếp theo thời gian tạo tăng dần.  
        /// - Nếu tài khoản không tồn tại, trả `404`. Nếu `page` hoặc `size` nhỏ hơn 1, trả `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="page">Số trang (mặc định 1).</param>
        /// <param name="size">Số lượng mỗi trang (mặc định 10).</param>
        /// <param name="id">ID của tài khoản.</param>
        /// <returns>
        /// - `200 OK`: Lấy danh sách ca làm việc thành công.  
        /// - `400 Bad Request`: Page hoặc size không hợp lệ.  
        /// - `404 Not Found`: Không tìm thấy tài khoản.
        /// </returns>
        [HttpGet(ApiEndPointConstant.WorkShift.GetWorkShiftByAccountId)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetWorkShiftByAccountId([FromQuery] int? page, [FromQuery] int? size, [FromRoute] Guid id)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _workShiftService.GetWorkShiftByAccountId(pageNumber, pageSize, id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// - API lấy danh sách ca làm việc còn trống theo ID tài khoản và ngày cụ thể với phân trang.
        /// - Dùng để lấy các ca làm việc chưa bị đặt của tham vấn viên trong ngày hôm đó cho người dùng chọn.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách ca làm việc còn trống của tài khoản, mặc định `page = 1`, `size = 10`.  
        /// - Chỉ lấy ca còn hoạt động (`IsActive = true`), khớp với ngày truyền vào, sắp xếp theo thời gian bắt đầu tăng dần.  
        /// - Lọc bỏ các ca đã được đặt trong bảng Booking dựa trên `id` và `date`.  
        /// - Nếu tài khoản không tồn tại, trả `404`. Nếu `page` hoặc `size` nhỏ hơn 1, trả `400`.  
        /// - Ví dụ: `date =2025-03-07T14:30:00` (Thứ Hai, 10/03/2025) sẽ lấy các ca trống của ngày Thứ Hai đó.
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="page">Số trang (mặc định 1).</param>
        /// <param name="size">Số lượng mỗi trang (mặc định 10).</param>
        /// <param name="id">ID của tài khoản tham vấn viên.</param>
        /// <param name="date">Ngày cần lấy ca trống (định dạng (yyyy-MM-ddTHH:mm:ssZ) ví dụ: 2025-03-07T14:30:00) THỜI GIAN KHÔNG QUAN TRỌNG, QUAN TRỌNG LÀ ĐÚNG NGÀY.</param>
        /// <returns>
        /// - `200 OK`: Lấy danh sách ca làm việc còn trống thành công.  
        /// - `400 Bad Request`: Page hoặc size không hợp lệ.  
        /// - `404 Not Found`: Không tìm thấy tài khoản.
        /// </returns>
        [HttpGet(ApiEndPointConstant.WorkShift.GetAvailableWorkShifts)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAvailableWorkShifts([FromQuery] int? page, [FromQuery] int? size, [FromRoute] Guid id, [FromQuery] DateTime date)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _workShiftService.GetAvailableWorkShifts(pageNumber, pageSize, id, date);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API cập nhật ca làm việc theo ID.
        /// </summary>
        /// <remarks>
        /// - Cập nhật ca làm việc dựa trên `id` và `request`.  
        /// - Nếu `id` không tồn tại hoặc không hoạt động, trả lỗi `404`.  
        /// - Nếu dữ liệu `request` không hợp lệ hoặc cập nhật thất bại, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của ca làm việc cần cập nhật.</param>
        /// <param name="request">Thông tin cập nhật cho ca làm việc.</param>
        /// <returns>
        /// - `200 OK`: Cập nhật ca làm việc thành công.  
        /// - `400 Bad Request`: Dữ liệu không hợp lệ hoặc cập nhật thất bại.  
        /// - `404 Not Found`: Không tìm thấy ca làm việc.
        /// </returns>
        [HttpPatch(ApiEndPointConstant.WorkShift.UpdateWorkShift)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateWorkShift([FromRoute] Guid id, [FromBody] UpdateWorkShiftRequest request, [FromQuery] DayEnum day)
        {
            if (request == null)
            {
                return BadRequest(new BaseResponse
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Invalid request data",
                    data = null
                });
            }
            var response = await _workShiftService.UpdateWorkShift(request, id, day);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API xoá ca làm việc theo ID.
        /// </summary>
        /// <remarks>
        /// - Xoá ca làm việc dựa trên `id` (chuyển `IsActive = false`).  
        /// - Nếu không tìm thấy ca làm việc, trả lỗi `404`.  
        /// - Nếu không xoá được, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của ca làm việc cần xoá.</param>
        /// <returns>
        /// - `200 OK`: Xoá ca làm việc thành công.  
        /// - `400 Bad Request`: Không thể xoá ca làm việc.  
        /// - `404 Not Found`: Không tìm thấy ca làm việc.
        /// </returns>
        [HttpDelete(ApiEndPointConstant.WorkShift.RemoveWorkShift)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RemoveWorkShift([FromRoute] Guid id)
        {
            var response = await _workShiftService.RemoveWorkShift(id);

            return StatusCode(int.Parse(response.status), response);
        }
    }
}
