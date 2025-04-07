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
            try
            {
                var googleAuthResponse = await _googleAuthenticationService.AuthenticateGoogleUser(HttpContext);

                var checkAccount = await _userService.GetAccountByEmail(googleAuthResponse.Email);
                if (!checkAccount)
                {
                    var response = await _userService.CreateNewUserAccountByGoogle(googleAuthResponse);
                    if (response == null)
                    {
                        string errorHtmlResponse = @"
                            <html>
                            <body>
                            <script type='text/javascript'>
                            window.opener.postMessage({ error: 'Tài khoản không tồn tại' }, '*');
                            window.close();
                            </script>
                            <p>Tài khoản google không tồn tại. Đang đóng cửa sổ...</p>
                            </body>
                            </html>";
                        return Content(errorHtmlResponse, "text/html");
                    }
                }

                var authResponse = await _userService.CreateTokenByEmail(googleAuthResponse.Email);
                googleAuthResponse.Token = authResponse.Token;
                googleAuthResponse.RefreshToken = authResponse.RefreshToken;

                if (authResponse == null || authResponse.Token == null || authResponse.RefreshToken == null)
                {
                    string errorHtmlResponse = @"
                        <html>
                        <body>
                        <script type='text/javascript'>
                        window.opener.postMessage({ error: 'Đăng nhập thất bại' }, '*');
                        window.close();
                        </script>
                        <p>Đăng nhập thất bại. Đang đóng cửa sổ...</p>
                        </body>
                        </html>";
                    return Content(errorHtmlResponse, "text/html");
                }

                var user = new
                {
                    accountId = authResponse.Id,
                    role = authResponse.Role,
                    username = authResponse.Username
                };

                string htmlResponse = $@"
                    <html>
                    <body>
                    <script type='text/javascript'>
                    window.opener.postMessage({{
                        user: '{user}'
                        accessToken: '{authResponse.Token}',
                        refreshToken: '{authResponse.RefreshToken}'
                    }}, '*');
                    window.close();
                    </script>
                    <p>Đang xử lý đăng nhập, vui lòng chờ...</p>
                    </body>
                    </html>";

                return Content(htmlResponse, "text/html");
            }
            catch (Exception ex)
            {
                string errorHtmlResponse = $@"
                    <html>
                    <body>
                    <script type='text/javascript'>
                    window.opener.postMessage({{ error: 'Đã xảy ra lỗi: {ex.Message}' }}, '*');
                    window.close();
                    </script>
                    <p>Đã xảy ra lỗi khi xử lý đăng nhập. Đang đóng cửa sổ...</p>
                    </body>
                    </html>";
                return Content(errorHtmlResponse, "text/html");
            }
        }
    }
}
