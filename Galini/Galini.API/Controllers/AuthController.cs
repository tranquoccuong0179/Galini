using Galini.API.Constants;
using Galini.Models.Payload.Request.Authenticaion;
using Galini.Models.Payload.Request.User;
using Galini.Models.Payload.Response;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class AuthController : BaseController<AuthController>
    {
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService) : base(logger)
        {
            _authService = authService;
        }

        /// <summary>
        /// API đăng nhập tài khoản vào hệ thống
        /// </summary>
        /// <remarks>
        /// - Yêu cầu thông tin người dùng thông qua `AuthenticateRequest`.
        /// - Trả về `BaseResponse` chứa kết quả đăng nhập.
        /// - Nếu đăng nhập thất bại trả về lỗi phù hợp.
        /// </remarks>
        /// <param name="request">Thông tin đăng nhập, bao gồm username và password.</param>
        /// <returns>
        /// - `200 OK`: Trả về danh sách tài khoản người nghe thành công.  
        /// - `400 Bad Request`: Thông tin không hợp lệ.
        /// </returns>
        [HttpPost(ApiEndPointConstant.Authentication.Authenticate)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Autheticate([FromBody] AuthenticateRequest request)
        {
            var response = await _authService.Authenticate(request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API xác thực người dùng bằng Refresh Token.
        /// </summary>
        /// <remarks>
        /// - Nhận Refresh Token từ yêu cầu.  
        /// - Trả về `BaseResponse` chứa Access Token mới nếu Refresh Token hợp lệ.  
        /// - Nếu Refresh Token không hợp lệ hoặc đã hết hạn, trả về lỗi `400 Bad Request`.  
        /// </remarks>
        /// <param name="request">Chuỗi Refresh Token.</param>
        /// <returns>
        /// - `200 OK`: Cấp lại Access Token thành công.  
        /// - `400 Bad Request`: Refresh Token không hợp lệ hoặc đã hết hạn.  
        /// </returns>
        [HttpPost(ApiEndPointConstant.Authentication.AutheticateWithRefreshToken)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> AutheticateWithRefreshToken([FromBody] string request)
        {
            var response = await _authService.AutheticateWithRefreshToken(request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API thu hồi tất cả Refresh Token của người dùng.
        /// </summary>
        /// <remarks>
        /// - Xóa toàn bộ Refresh Token của người dùng dựa trên `accountId`.  
        /// - Nếu tài khoản không tồn tại, trả về lỗi `400 Bad Request`.  
        /// - Nếu không có Refresh Token nào để thu hồi, trả về `200 OK`.  
        /// - Nếu thu hồi thành công, trả về `200 OK`.  
        /// - Nếu có lỗi trong quá trình xử lý, trả về `400 Bad Request`.  
        /// </remarks>
        /// <param name="id">ID của tài khoản cần thu hồi Refresh Token.</param>
        /// <returns>
        /// - `200 OK`: Thu hồi thành công hoặc không có Refresh Token nào để xóa.  
        /// - `400 Bad Request`: Tài khoản không tồn tại hoặc có lỗi khi thu hồi Refresh Token.  
        /// </returns>
        [HttpPost(ApiEndPointConstant.Authentication.RevokeRefreshToken)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RevokeRefreshToken([FromRoute] Guid id)
        {
            var response = await _authService.RevokeRefreshToken(id);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}
