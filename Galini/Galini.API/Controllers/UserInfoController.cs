using Galini.API.Constants;
using Galini.Models.Entity;
using Galini.Models.Payload.Request.User;
using Galini.Models.Payload.Request.UserInfo;
using Galini.Models.Payload.Response;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Galini.API.Controllers
{
    public class UserInfoController : BaseController<UserInfoController>
    {
        private readonly IUserInfoService _userInfoService;
        public UserInfoController(ILogger<UserInfoController> logger, IUserInfoService userInfoService) : base(logger)
        {
            _userInfoService = userInfoService;
        }

        /// <summary>
        /// API tạo thông tin người dùng mới, đây cũng là nơi lưu thông tin premium của user.
        /// </summary>
        /// <remarks>
        /// - Tạo thông tin người dùng dựa trên `request`, `accountId` và `premiumId`.  
        /// - Kiểm tra tài khoản và gói premium có tồn tại không, nếu không trả `404`.  
        /// - Nếu dữ liệu `request` không hợp lệ, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="request">Dữ liệu thông tin người dùng cần tạo.</param>
        /// <param name="accountId">ID của tài khoản người dùng.</param>
        /// <param name="premiumId">ID của gói premium.</param>
        /// <returns>
        /// - `200 OK`: Tạo thông tin người dùng thành công.  
        /// - `400 Bad Request`: Dữ liệu không hợp lệ hoặc lưu thất bại.  
        /// - `404 Not Found`: Không tìm thấy tài khoản hoặc gói premium.
        /// </returns>
        [HttpPost(ApiEndPointConstant.UserInfo.CreateUserInfo)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateUserInfo([FromBody] CreateUserInfoRequest request, [FromQuery] Guid accountId, [FromQuery] Guid premiumId)
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

            var response = await _userInfoService.CreateUserInfo(request, accountId, premiumId);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách thông tin người dùng với phân trang.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách thông tin người dùng còn hoạt động, mặc định `page = 1`, `size = 10`.  
        /// - Lọc theo loại premium (`premium`) nếu có, sắp xếp theo premium (`sortByPremium`) hoặc thời gian tạo.  
        /// - Nếu `sortByPremium = true` thì tăng dần, `false` thì giảm dần, không truyền thì theo thời gian tạo.  
        /// - Nếu `page` hoặc `size` nhỏ hơn 1, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="page">Số trang (mặc định 1).</param>
        /// <param name="size">Số lượng mỗi trang (mặc định 10).</param>
        /// <param name="premium">Lọc theo loại premium.</param>
        /// <param name="sortByPremium">Sắp xếp theo premium (true: tăng, false: giảm).</param>
        /// <returns>
        /// - `200 OK`: Lấy danh sách thông tin người dùng thành công.  
        /// - `400 Bad Request`: Page hoặc size không hợp lệ.
        /// </returns>
        [HttpGet(ApiEndPointConstant.UserInfo.GetAllUserInfo)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllUserInfo([FromQuery] int? page, [FromQuery] int? size, [FromQuery] string? premium, [FromQuery] bool? sortByPremium)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _userInfoService.GetAllUserInfo(pageNumber, pageSize, premium, sortByPremium);

            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy thông tin người dùng theo ID tài khoản.
        /// </summary>
        /// <remarks>
        /// - Trả về thông tin người dùng đầu tiên dựa trên `id` tài khoản.  
        /// - Chỉ lấy nếu tài khoản và thông tin còn hoạt động (`IsActive = true`).  
        /// - Nếu không tìm thấy tài khoản hoặc thông tin người dùng, trả lỗi `404`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của tài khoản.</param>
        /// <returns>
        /// - `200 OK`: Lấy thông tin người dùng thành công.  
        /// - `404 Not Found`: Không tìm thấy tài khoản hoặc thông tin người dùng.
        /// </returns>
        [HttpGet(ApiEndPointConstant.UserInfo.GetUserInfoByAccountId)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetUserInfoByAccountId([FromRoute] Guid id)
        {
            var response = await _userInfoService.GetUserInfoByAccountId(id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy thông tin người dùng theo ID.
        /// </summary>
        /// <remarks>
        /// - Trả về chi tiết thông tin người dùng dựa trên `id`.  
        /// - Chỉ lấy nếu còn hoạt động (`IsActive = true`).  
        /// - Nếu không tìm thấy, trả lỗi `404`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của thông tin người dùng.</param>
        /// <returns>
        /// - `200 OK`: Lấy thông tin người dùng thành công.  
        /// - `404 Not Found`: Không tìm thấy thông tin người dùng.
        /// </returns>
        [HttpGet(ApiEndPointConstant.UserInfo.GetUserInfoById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetUserInfoById([FromRoute] Guid id)
        {
            var response = await _userInfoService.GetUserInfoById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// - API cập nhật thông tin người dùng theo ID.
        /// - Nếu người dùng muốn mua premium mới, hoặc update premium của người dùng thì sử dụng API nàys
        /// </summary>
        /// <remarks>
        /// - Cập nhật thông tin người dùng dựa trên `id`, `premiumId` và `request`.  
        /// - Nếu `id` không tồn tại hoặc không hoạt động, trả lỗi `404`.  
        /// - Nếu dữ liệu không hợp lệ hoặc cập nhật thất bại, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của thông tin người dùng cần cập nhật.</param>
        /// <param name="premiumId">ID của gói premium.</param>
        /// <param name="request">Thông tin cập nhật cho người dùng.</param>
        /// <returns>
        /// - `200 OK`: Cập nhật thông tin người dùng thành công.  
        /// - `400 Bad Request`: Dữ liệu không hợp lệ hoặc cập nhật thất bại.  
        /// - `404 Not Found`: Không tìm thấy thông tin người dùng.
        /// </returns>
        [HttpPatch(ApiEndPointConstant.UserInfo.UpdateUserInfo)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateUserInfo([FromRoute] Guid id, [FromQuery] Guid premiumId, [FromBody] UpdateUserInfoRequest request)
        {

            var response = await _userInfoService.UpdateUserInfo(id, premiumId, request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API xoá thông tin người dùng theo ID.
        /// </summary>
        /// <remarks>
        /// - Xoá thông tin người dùng dựa trên `id` (chuyển `IsActive = false`).  
        /// - Nếu không tìm thấy thông tin người dùng, trả lỗi `404`.  
        /// - Nếu không xoá được, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của thông tin người dùng cần xoá.</param>
        /// <returns>
        /// - `200 OK`: Xoá thông tin người dùng thành công.  
        /// - `400 Bad Request`: Không thể xoá thông tin người dùng.  
        /// - `404 Not Found`: Không tìm thấy thông tin người dùng.
        /// </returns>
        [HttpDelete(ApiEndPointConstant.UserInfo.RemoveUserInfo)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RemoveUserInfo([FromRoute] Guid id)
        {
            var response = await _userInfoService.RemoveUserInfo(id);

            return StatusCode(int.Parse(response.status), response);
        }
    }
}
