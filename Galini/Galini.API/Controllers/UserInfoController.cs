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

        [HttpGet(ApiEndPointConstant.UserInfo.GetAllUserInfo)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllUserInfo([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _userInfoService.GetAllUserInfo(pageNumber, pageSize);

            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.UserInfo.GetUserInfoByAccountId)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetUserInfoByAccountId([FromRoute] Guid id)
        {
            var response = await _userInfoService.GetUserInfoByAccountId(id);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.UserInfo.GetUserInfoById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetUserInfoById([FromRoute] Guid id)
        {
            var response = await _userInfoService.GetUserInfoById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpPatch(ApiEndPointConstant.UserInfo.UpdateUserInfo)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateUserInfo([FromRoute] Guid id, [FromQuery] Guid premiumId, [FromBody] UpdateUserInfoRequest request)
        {

            var response = await _userInfoService.UpdateUserInfo(id, premiumId, request);
            return StatusCode(int.Parse(response.status), response);
        }

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
