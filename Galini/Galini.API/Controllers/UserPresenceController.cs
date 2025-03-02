using Galini.API.Constants;
using Galini.Models.Payload.Request.CallHistory;
using Galini.Models.Payload.Request.UserPresence;
using Galini.Models.Payload.Response;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class UserPresenceController : BaseController<UserPresenceController>
    {
        private readonly IUserPresenceService _userPresenceService;
        public UserPresenceController(ILogger<UserPresenceController> logger, IUserPresenceService userPresenceService) : base(logger)
        {
            _userPresenceService = userPresenceService;
        }

        /// <summary>
        /// API tạo trạng thái người dùng mới, tạo ngay khi người dùng create, và đã verify tài khoải.
        /// </summary>
        /// <remarks>
        /// - Tạo trạng thái người dùng dựa trên `request` và `id` tài khoản.  
        /// - Kiểm tra tài khoản có tồn tại không, nếu không trả `404`.  
        /// - Nếu dữ liệu `request` không hợp lệ, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="request">Dữ liệu trạng thái người dùng cần tạo.</param>
        /// <param name="id">ID của tài khoản người dùng.</param>
        /// <returns>
        /// - `200 OK`: Tạo trạng thái người dùng thành công.  
        /// - `400 Bad Request`: Dữ liệu không hợp lệ hoặc lưu thất bại.  
        /// - `404 Not Found`: Không tìm thấy tài khoản.
        /// </returns>
        [HttpPost(ApiEndPointConstant.UserPresence.CreateUserPresence)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateUserPresence([FromBody] CreateUserPresenceRequest request, [FromRoute] Guid id)
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

            var response = await _userPresenceService.CreateUserPresence(request, id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách trạng thái người dùng với phân trang.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách trạng thái người dùng còn hoạt động, mặc định `page = 1`, `size = 10`.  
        /// - Sắp xếp theo thời gian tạo tăng dần, không hỗ trợ lọc hay sắp xếp tùy chỉnh.  
        /// - Nếu `page` hoặc `size` nhỏ hơn 1, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="page">Số trang (mặc định 1).</param>
        /// <param name="size">Số lượng mỗi trang (mặc định 10).</param>
        /// <returns>
        /// - `200 OK`: Lấy danh sách trạng thái người dùng thành công.  
        /// - `400 Bad Request`: Page hoặc size không hợp lệ.
        /// </returns>
        [HttpGet(ApiEndPointConstant.UserPresence.GetAllUserPresence)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllUserPresence([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _userPresenceService.GetAllUserPresence(pageNumber, pageSize);

            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy trạng thái người dùng theo ID, được sử dụng để thể hiện trạng thái của bạn bè có online, offline, incall.
        /// </summary>
        /// <remarks>
        /// - Trả về chi tiết trạng thái người dùng dựa trên `id`.  
        /// - Chỉ lấy nếu còn hoạt động (`IsActive = true`).  
        /// - Nếu không tìm thấy, trả lỗi `404`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của trạng thái người dùng.</param>
        /// <returns>
        /// - `200 OK`: Lấy trạng thái người dùng thành công.  
        /// - `404 Not Found`: Không tìm thấy trạng thái người dùng.
        /// </returns>
        [HttpGet(ApiEndPointConstant.UserPresence.GetUserPresenceById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetUserPresenceById([FromRoute] Guid id)
        {
            var response = await _userPresenceService.GetUserPresenceById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy trạng thái người dùng theo ID tài khoản.
        /// </summary>
        /// <remarks>
        /// - Trả về trạng thái người dùng đầu tiên dựa trên `id` tài khoản.  
        /// - Chỉ lấy nếu tài khoản và trạng thái còn hoạt động (`IsActive = true`).  
        /// - Nếu không tìm thấy tài khoản hoặc trạng thái, trả lỗi `404`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của tài khoản.</param>
        /// <returns>
        /// - `200 OK`: Lấy trạng thái người dùng thành công.  
        /// - `404 Not Found`: Không tìm thấy tài khoản hoặc trạng thái người dùng.
        /// </returns>
        [HttpGet(ApiEndPointConstant.UserPresence.GetUserPresenceByAccountId)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetUserPresenceByAccountId([FromRoute] Guid id)
        {
            var response = await _userPresenceService.GetUserPresenceByAccountId(id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API cập nhật trạng thái người dùng theo ID.
        /// </summary>
        /// <remarks>
        /// - Cập nhật trạng thái người dùng dựa trên `id` và `request`.  
        /// - Nếu `id` không tồn tại hoặc không hoạt động, trả lỗi `404`.  
        /// - Nếu dữ liệu `request` không hợp lệ hoặc cập nhật thất bại, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của trạng thái người dùng cần cập nhật.</param>
        /// <param name="request">Thông tin cập nhật cho trạng thái người dùng.</param>
        /// <returns>
        /// - `200 OK`: Cập nhật trạng thái người dùng thành công.  
        /// - `400 Bad Request`: Dữ liệu không hợp lệ hoặc cập nhật thất bại.  
        /// - `404 Not Found`: Không tìm thấy trạng thái người dùng.
        /// </returns>
        [HttpPatch(ApiEndPointConstant.UserPresence.UpdateUserPresence)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateUserPresence([FromRoute] Guid id, [FromBody] UpdateUserPresenceRequest request)
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
            var response = await _userPresenceService.UpdateUserPresence(id, request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API xoá trạng thái người dùng theo ID.
        /// </summary>
        /// <remarks>
        /// - Xoá trạng thái người dùng dựa trên `id` (chuyển `IsActive = false`).  
        /// - Nếu không tìm thấy trạng thái người dùng, trả lỗi `404`.  
        /// - Nếu không xoá được, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của trạng thái người dùng cần xoá.</param>
        /// <returns>
        /// - `200 OK`: Xoá trạng thái người dùng thành công.  
        /// - `400 Bad Request`: Không thể xoá trạng thái người dùng.  
        /// - `404 Not Found`: Không tìm thấy trạng thái người dùng.
        /// </returns>
        [HttpDelete(ApiEndPointConstant.UserPresence.RemoveUserPresence)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RemoveUserPresence([FromRoute] Guid id)
        {
            var response = await _userPresenceService.RemoveUserPresence(id);

            return StatusCode(int.Parse(response.status), response);
        }
    }
}
