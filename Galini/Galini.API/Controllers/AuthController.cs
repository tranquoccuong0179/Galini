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

        [HttpPost(ApiEndPointConstant.Authentication.AutheticateWithRefreshToken)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> AutheticateWithRefreshToken([FromBody] string request)
        {
            var response = await _authService.AutheticateWithRefreshToken(request);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}
