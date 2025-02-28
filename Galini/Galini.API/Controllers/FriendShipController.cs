using Galini.API.Constants;
using Galini.Models.Enum;
using Galini.Models.Payload.Request.CallHistory;
using Galini.Models.Payload.Request.FriendShip;
using Galini.Models.Payload.Request.UserInfo;
using Galini.Models.Payload.Response;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class FriendShipController : BaseController<FriendShipController>
    {
        private readonly IFriendShipService _friendShipService;
        public FriendShipController(ILogger<FriendShipController> logger, IFriendShipService friendShipService) : base(logger)
        {
            _friendShipService = friendShipService;
        }

        [HttpPost(ApiEndPointConstant.FriendShip.CreateFriendShip)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateFriendShip([FromQuery] Guid userId, [FromQuery] Guid friendId)
        {
            var response = await _friendShipService.CreateFriendShip(userId, friendId);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.FriendShip.GetAllFriendShip)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllFriendShip([FromQuery] int? page, [FromQuery] int? size, [FromQuery] string? status, [FromQuery] bool? sortByStatus)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _friendShipService.GetAllFriendShip(pageNumber, pageSize, status, sortByStatus);

            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.FriendShip.GetFriendList)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetFriendList([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _friendShipService.GetFriendList(pageNumber, pageSize);

            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.FriendShip.GetFriendShipById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetFriendShipById([FromRoute] Guid id)
        {
            var response = await _friendShipService.GetFriendShipById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.FriendShip.GetFriendShipByAccountIdAndStatus)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetFriendShipByAccountIdAndStatus([FromRoute] Guid id, [FromQuery] FriendShipEnum status, [FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _friendShipService.GetFriendShipByAccountIdAndStatus(id, status, pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.FriendShip.GetFriendByAccountId)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetFriendByAccountId([FromRoute] Guid id, [FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _friendShipService.GetFriendByAccountId(id, pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpPatch(ApiEndPointConstant.FriendShip.UpdateFriendShip)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateFriendShip([FromRoute] Guid id, [FromBody] UpdateFriendShipRequest request)
        {

            var response = await _friendShipService.UpdateFriendShip(id, request);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpDelete(ApiEndPointConstant.FriendShip.RemoveFriendShip)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RemoveFriendShip([FromRoute] Guid id)
        {
            var response = await _friendShipService.RemoveFriendShip(id);

            return StatusCode(int.Parse(response.status), response);
        }
    }
}
