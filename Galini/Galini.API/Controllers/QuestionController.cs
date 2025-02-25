using Galini.API.Constants;
using Galini.API.Infrastructure;
using Galini.Models.Enum;
using Galini.Models.Payload.Request.Notification;
using Galini.Models.Payload.Request.Question;
using Galini.Models.Payload.Response;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class QuestionController : BaseController<QuestionController>
    {
        private readonly IQuestionService _questionService;
        public QuestionController(ILogger<QuestionController> logger, IQuestionService questionService) : base(logger)
        {
            _questionService = questionService;
        }

        [HttpPost(ApiEndPointConstant.Question.CreateQuestion)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionRequest request)
        {
            var response = await _questionService.CreateQuestion(request);
            return StatusCode(int.Parse(response.status), response);
        }


        [HttpGet(ApiEndPointConstant.Question.GetAllQuestion)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetNotifications([FromQuery] int? page,
                                                          [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _questionService.GetAllQuestion(pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.Question.GetQuestionById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetQuestionById([FromRoute] Guid id)
        {
            var response = await _questionService.GetQuestionById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpDelete(ApiEndPointConstant.Question.RemoveQuestion)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RemoveQuestion([FromRoute] Guid id)
        {
            var response = await _questionService.RemoveQuestion(id);
            return StatusCode(int.Parse(response.status), response);
        }
        
        [HttpPut(ApiEndPointConstant.Question.UpdateQuestion)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateQuestion([FromRoute] Guid id, [FromBody] UpdateQuestionRequest request)
        {
            var response = await _questionService.UpdateQuestion(id, request);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}
