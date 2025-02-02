
using System.Drawing;
using Galini.API.Constants;
using Galini.Models.Payload.Request.Topic;
using Galini.Models.Payload.Response;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Galini.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class TopicController : BaseController<AccountController>
    {
        private readonly ITopicService _service;
        public TopicController(ILogger<AccountController> logger, ITopicService service) : base(logger)
        {
            _service = service;
        }

        [HttpPost(ApiEndPointConstant.Topic.CreateTopic)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateTopic([FromBody] CreateTopicRequest request)
        {
            var response = await _service.CreateTopic(request);
            return StatusCode(int.Parse(response.status), response);
        }
        
        [HttpGet(ApiEndPointConstant.Topic.GetListTopic)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetTopics([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _service.GetAllTopic(pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }
        
        [HttpGet(ApiEndPointConstant.Topic.GetTopic)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetTopics([FromRoute] Guid id)
        {
            var response = await _service.GetTopicById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpPut(ApiEndPointConstant.Topic.UpdateTopic)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateTopic([FromRoute] Guid id, [FromBody] UpdateTopicRequest request)
        {
            var response = await _service.UpdateTopic(id, request);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpDelete(ApiEndPointConstant.Topic.DeleteTopic)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteTopic([FromRoute] Guid id)
        {
            var response = await _service.RemoveTopic(id);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}
