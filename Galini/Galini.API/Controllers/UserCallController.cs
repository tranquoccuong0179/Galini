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

        [HttpGet(ApiEndPointConstant.UserCall.GetUserCallByAccountId)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetUserCallByAccountId([FromRoute] Guid id)
        {
            var response = await _userCallService.GetUserCallByAccountId(id);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.UserCall.GetUserCallById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetUserCallById([FromRoute] Guid id)
        {
            var response = await _userCallService.GetUserCallById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpPatch(ApiEndPointConstant.UserCall.UpdateUserCall)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateUserCall([FromRoute] Guid id, [FromBody] UpdateUserCallRequest request, [FromQuery] Guid accountId, [FromQuery] Guid callHistoryId)
        {

            var response = await _userCallService.UpdateUserCall(id, request, accountId, callHistoryId);
            return StatusCode(int.Parse(response.status), response);
        }

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
