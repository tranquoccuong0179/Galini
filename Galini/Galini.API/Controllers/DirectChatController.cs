
using Galini.API.Constants;
using Galini.Models.Payload.Request.DirectChat;
using Galini.Models.Payload.Response;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class DirectChatController : BaseController<DirectChatController>
    {
        private readonly IDirectChatService _directChat;
        public DirectChatController(ILogger<DirectChatController> logger, IDirectChatService directChat) : base(logger)
        {
            _directChat = directChat;
        }

        [HttpPost(ApiEndPointConstant.DirectChat.CreateDirectChat)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateDirectChat([FromBody] CreateDirectChatRequest request)
        {
            var response = await _directChat.CreateDirectChat(request);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.DirectChat.GetAllDirectChats)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllDirectChats([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _directChat.GetAllDirectChat(pageNumber, pageSize);

            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.DirectChat.GetDirectChatById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetDirectChatById([FromRoute] Guid id)
        {
            var response = await _directChat.GetDirectChatById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpPatch(ApiEndPointConstant.DirectChat.UpdateDirectChat)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateDirectChat([FromRoute] Guid id, [FromBody] UpdateDirectChatRequest request)
        {

            var response = await _directChat.UpdateDirectChat(id, request);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpDelete(ApiEndPointConstant.DirectChat.RemoveDirectChat)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RemoveDirectChat([FromRoute] Guid id)
        {
            var response = await _directChat.RemoveDirectChat(id);

            return StatusCode(int.Parse(response.status), response);
        }
    }
}
