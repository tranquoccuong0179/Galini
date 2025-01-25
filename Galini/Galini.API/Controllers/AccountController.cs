using Galini.API.Constants;
using Galini.Models.Payload.Request.User;
using Galini.Models.Payload.Response;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers;

public class AccountController : BaseController<AccountController>
{
    private readonly IUserService _userService;
    public AccountController(ILogger<AccountController> logger, IUserService userService) : base(logger)
    {
        _userService = userService;
    }
    
    [HttpPost(ApiEndPointConstant.User.RegisterUser)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    public async Task<IActionResult> CreateNewAccount([FromBody] RegisterUserRequest request)
    {
        var response = await _userService.RegisterUser(request);
        return StatusCode(int.Parse(response.status), response);
    }
}