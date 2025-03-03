using Galini.API.Constants;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Galini.Models.Payload.Response;

namespace Galini.API.Controllers
{
    [ApiController]
    [Route(ApiEndPointConstant.GoogleAuthentication.GoogleAuthenticationEndPoint)]
    public class GoogleAuthenticationController : BaseController<GoogleAuthenticationController>
    {
        private readonly IUserService _userService;
        private readonly IGoogleAuthenticationService _googleAuthenticationService;

        public GoogleAuthenticationController(ILogger<GoogleAuthenticationController> logger, IUserService userService, IGoogleAuthenticationService googleAuthenticationService) : base(logger)
        {
            _userService = userService;
            _googleAuthenticationService = googleAuthenticationService;
        }

        /// <summary>
        /// API đăng nhập bằng google.
        /// </summary>
        /// <returns>
        /// - `200 OK`: Đăng nhập thành công.  
        /// </returns>
        [HttpGet(ApiEndPointConstant.GoogleAuthentication.GoogleLogin)]
        public IActionResult Login()
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = $"api/v1/google-auth/signin-google"
            };
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet(ApiEndPointConstant.GoogleAuthentication.GoogleSignIn)]
        public async Task<IActionResult> SignInGoogle()
        {
            var googleAuthResponse = await _googleAuthenticationService.AuthenticateGoogleUser(HttpContext);
            var checkAccount = await _userService.GetAccountByEmail(googleAuthResponse.Email);
            if (!checkAccount)
            {
                var response = await _userService.CreateNewUserAccountByGoogle(googleAuthResponse);
                if (response == null)
                {
                    return Problem("Tài khoản không tồn tại");
                }
            }
            var authResponse = await _userService.CreateTokenByEmail(googleAuthResponse.Email);
            googleAuthResponse.Token = authResponse.Token;
            googleAuthResponse.RefreshToken = authResponse.RefreshToken;

            if (authResponse == null || authResponse.Token == null || authResponse.RefreshToken == null)
            {
                return Ok(new BaseResponse()
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Login faild",
                    data = null
                });
            }

            return Ok(new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Login successful",
                data = googleAuthResponse
            });
        }
    }
}
