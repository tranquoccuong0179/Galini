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

        [HttpPost(ApiEndPointConstant.Authentication.Authenticate)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Autheticate([FromBody] AuthenticateRequest request)
        {
            var response = await _authService.Authenticate(request);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}
