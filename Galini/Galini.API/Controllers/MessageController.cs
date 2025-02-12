using Galini.API.Constants;
using Galini.Models.Payload.Request.CallHistory;
using Galini.Models.Payload.Request.Message;
using Galini.Models.Payload.Response;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class MessageController : BaseController<MessageController>
    {
        private readonly IMessageService _messageService;

        public MessageController(ILogger<MessageController> logger, IMessageService messageService) : base(logger)
        {
            _messageService = messageService;
        }

        [HttpPost(ApiEndPointConstant.Message.CreateMessage)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateMessage([FromBody] CreateMessageRequest request, [FromQuery] Guid id)
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

            var response = await _messageService.CreateMessage(request, id);
            return StatusCode(int.Parse(response.status), response);
        }
        
        [HttpPost(ApiEndPointConstant.Message.CreateMessageCall)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateMessageCall([FromBody] CreateMessageRequest request, [FromQuery] Guid id)
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

            var response = await _messageService.CreateMessageCall(request, id);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.Message.GetAllMessage)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllMessage([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _messageService.GetAllMessage(pageNumber, pageSize);

            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.Message.GetMessageByDirectChatId)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetMessageByDirectChatId([FromRoute] Guid id, [FromBody] DateTime? beforeCursor)
        {
            var response = await _messageService.GetMessageByDirectChatId(id, beforeCursor);

            return StatusCode(int.Parse(response.status), response);
        }
        
        [HttpGet(ApiEndPointConstant.Message.GetMessageById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetMessageById([FromRoute] Guid id)
        {
            var response = await _messageService.GetMessageById(id);

            return StatusCode(int.Parse(response.status), response);
        }

        [HttpPatch(ApiEndPointConstant.Message.UpdateMessage)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateMessage([FromRoute] Guid id, [FromBody] UpdateMessageRequest request)
        {

            var response = await _messageService.UpdateMessage(id, request);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpDelete(ApiEndPointConstant.Message.RemoveMessage)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RemoveMessage([FromRoute] Guid id)
        {
            var response = await _messageService.RemoveMessage(id);

            return StatusCode(int.Parse(response.status), response);
        }
    }
}
