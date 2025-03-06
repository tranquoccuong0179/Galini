
using System.Drawing;
using Galini.API.Constants;
using Galini.Models.Payload.Request.DirectChat;
using Galini.Models.Payload.Request.DirectChatParticipant;
using Galini.Models.Payload.Response;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class DirectChatParticipantController : BaseController<DirectChatParticipantController>
    {
        private readonly IDirectChatParticipantService _directChatParticipantService;
        public DirectChatParticipantController(ILogger<DirectChatParticipantController> logger, IDirectChatParticipantService directChatParticipantService) : base(logger)
        {
            _directChatParticipantService = directChatParticipantService;
        }

        [HttpPost(ApiEndPointConstant.DirectChatParticipant.CreateDirectChatParticipant)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateDirectChatParticipant([FromBody] CreateDirectChatParticipant request)
        {
            var response = await _directChatParticipantService.CreateDirectChatParticipant(request);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.DirectChatParticipant.GetAllDirectChatParticipants)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllDirectChatParticipants([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _directChatParticipantService.GetAllDirectChatParticipant(pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.DirectChatParticipant.GetDirectChatParticipantById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetDirectChatParticipantById([FromRoute] Guid id)
        {
            var response = await _directChatParticipantService.GetDirectChatParticipantById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpPut(ApiEndPointConstant.DirectChatParticipant.UpdateDirectChatParticipant)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateDirectChatParticipant([FromRoute] Guid id, [FromBody] UpdateDirectChatParticipant request)
        {
            var response = await _directChatParticipantService.UpdateDirectChatParticipant(id, request);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpDelete(ApiEndPointConstant.DirectChatParticipant.RemoveDirectChatParticipant)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RemoveDirectChatParticipant([FromRoute] Guid id)
        {
            var response = await _directChatParticipantService.RemoveDirectChatParticipant(id);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}
