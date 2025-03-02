using Galini.API.Constants;
using Galini.Models.Payload.Request.UserCall;
using Galini.Models.Payload.Request.UserInfo;
using Galini.Models.Payload.Response;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class UserCallController : BaseController<UserCallController>
    {
        private readonly IUserCallService _userCallService;
        public UserCallController(ILogger<UserInfoController> logger, IUserCallService userCallService) : base(logger)
        {
            _userCallService = userCallService;
        }

        /// <summary>
        /// - API tạo thông tin người dùng tham gia cuộc gọi mới cho người dùng.
        /// - Đây là API lưu thông tin của người dùng đã tham gia cuộc gọi, không phải lịch sử cuộc gọi
        /// </summary>
        /// <remarks>
        /// - Tạo thông tin người dùng tham gia cuộc gọi dựa trên `request`, `accountId` và `callHistoryId`.  
        /// - Kiểm tra tài khoản và lịch sử cuộc gọi có tồn tại không, nếu không trả `404`.  
        /// - Nếu dữ liệu `request` không hợp lệ, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="request">Dữ liệu lịch sử cuộc gọi cần tạo.</param>
        /// <param name="accountId">ID của tài khoản người dùng.</param>
        /// <param name="callHistoryId">ID của lịch sử cuộc gọi.</param>
        /// <returns>
        /// - `200 OK`: Tạo lịch sử cuộc gọi thành công.  
        /// - `400 Bad Request`: Dữ liệu không hợp lệ hoặc lưu thất bại.  
        /// - `404 Not Found`: Không tìm thấy tài khoản hoặc lịch sử cuộc gọi.
        /// </returns>
        [HttpPost(ApiEndPointConstant.UserCall.CreateUserCall)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateUserCall([FromBody] CreateUserCallRequest request, [FromQuery] Guid accountId, [FromQuery] Guid callHistoryId)
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

            var response = await _userCallService.CreateUserCall(request, accountId, callHistoryId);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách thông tin người dùng tham gia cuộc gọi với phân trang.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách thông tin người dùng tham gia cuộc gọi còn hoạt động, mặc định `page = 1`, `size = 10`.  
        /// - Nếu `page` hoặc `size` nhỏ hơn 1, trả lỗi `400`.  
        /// - Không hỗ trợ lọc hay sắp xếp tùy chỉnh, mặc định theo thứ tự repository.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="page">Số trang (mặc định 1).</param>
        /// <param name="size">Số lượng mỗi trang (mặc định 10).</param>
        /// <returns>
        /// - `200 OK`: Lấy danh sách thông tin người dùng tham gia cuộc gọi cuộc gọi thành công.  
        /// - `400 Bad Request`: Page hoặc size không hợp lệ.
        /// </returns>
        [HttpGet(ApiEndPointConstant.UserCall.GetAllUserCall)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllUserCall([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _userCallService.GetAllUserCall(pageNumber, pageSize);

            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy thông tin người dùng tham gia cuộc gọi theo ID tài khoản.
        /// </summary>
        /// <remarks>
        /// - Trả về thông tin người dùng tham gia cuộc gọi của tài khoản dựa trên `id`.  
        /// - Chỉ lấy nếu tài khoản và thông tin người dùng tham gia cuộc gọi còn hoạt động (`IsActive = true`).  
        /// - Nếu không tìm thấy tài khoản hoặc thông tin người dùng tham gia cuộc gọi, trả lỗi `404`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của tài khoản.</param>
        /// <returns>
        /// - `200 OK`: Lấy thông tin người dùng tham gia cuộc gọi thành công.  
        /// - `404 Not Found`: Không tìm thấy tài khoản hoặc thông tin người dùng tham gia cuộc gọi.
        /// </returns>
        [HttpGet(ApiEndPointConstant.UserCall.GetUserCallByAccountId)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetUserCallByAccountId([FromQuery] int? page, [FromQuery] int? size, [FromRoute] Guid id)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _userCallService.GetUserCallByAccountId(pageNumber, pageSize, id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy thông tin người dùng tham gia cuộc gọi cuộc gọi theo ID.
        /// </summary>
        /// <remarks>
        /// - Trả về chi tiết thông tin người dùng tham gia cuộc gọi dựa trên `id`.  
        /// - Chỉ lấy nếu còn hoạt động (`IsActive = true`).  
        /// - Nếu không tìm thấy, trả lỗi `404`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của lịch sử cuộc gọi.</param>
        /// <returns>
        /// - `200 OK`: Lấy thông tin người dùng tham gia cuộc gọi thành công.  
        /// - `404 Not Found`: Không tìm thấy thông tin người dùng tham gia cuộc gọi.
        /// </returns>
        [HttpGet(ApiEndPointConstant.UserCall.GetUserCallById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetUserCallById([FromRoute] Guid id)
        {
            var response = await _userCallService.GetUserCallById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API cập nhật thông tin người dùng tham gia cuộc gọi theo ID.
        /// </summary>
        /// <remarks>
        /// - Cập nhật thông tin người dùng tham gia cuộc gọi dựa trên `id`, `request`, `accountId` và `callHistoryId`.  
        /// - Nếu `id` không tồn tại hoặc không hoạt động, trả lỗi `404`.  
        /// - Nếu dữ liệu không hợp lệ hoặc cập nhật thất bại, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của lịch sử cuộc gọi cần cập nhật.</param>
        /// <param name="request">Thông tin cập nhật cho thông tin người dùng tham gia cuộc gọi.</param>
        /// <param name="accountId">ID của tài khoản người dùng.</param>
        /// <param name="callHistoryId">ID của lịch sử cuộc gọi.</param>
        /// <returns>
        /// - `200 OK`: Cập nhật thông tin người dùng tham gia cuộc gọi thành công.  
        /// - `400 Bad Request`: Dữ liệu không hợp lệ hoặc cập nhật thất bại.  
        /// - `404 Not Found`: Không tìm thấy thông tin người dùng tham gia cuộc gọi.
        /// </returns>
        [HttpPatch(ApiEndPointConstant.UserCall.UpdateUserCall)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateUserCall([FromRoute] Guid id, [FromBody] UpdateUserCallRequest request, [FromQuery] Guid accountId, [FromQuery] Guid callHistoryId)
        {

            var response = await _userCallService.UpdateUserCall(id, request, accountId, callHistoryId);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API xoá thông tin người dùng tham gia cuộc gọi theo ID.
        /// </summary>
        /// <remarks>
        /// - Xoá thông tin người dùng tham gia cuộc gọi dựa trên `id` (chuyển `IsActive = false`).  
        /// - Nếu không tìm thấy thông tin người dùng tham gia cuộc gọi, trả lỗi `404`.  
        /// - Nếu không xoá được, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của lịch sử cuộc gọi cần xoá.</param>
        /// <returns>
        /// - `200 OK`: Xoá thông tin người dùng tham gia cuộc gọi thành công.  
        /// - `400 Bad Request`: Không thể xóa thông tin người dùng tham gia cuộc gọi.  
        /// - `404 Not Found`: Không tìm thấy thông tin người dùng tham gia cuộc gọi.
        /// </returns>
        [HttpDelete(ApiEndPointConstant.UserCall.RemoveUserCall)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RemoveUserCall([FromRoute] Guid id)
        {
            var response = await _userCallService.RemoveUserCall(id);

            return StatusCode(int.Parse(response.status), response);
        }
    }
}
