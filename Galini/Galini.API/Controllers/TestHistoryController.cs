using Galini.API.Constants;
using Galini.Models.Payload.Request.TestHistory;
using Galini.Models.Payload.Response;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class TestHistoryController : BaseController<TestHistoryController>
    {
        private readonly ITestHistoryService _testHistoryService;
        public TestHistoryController(ILogger<TestHistoryController> logger, ITestHistoryService testHistoryService) : base(logger)
        {
            _testHistoryService = testHistoryService;
        }

        [HttpPost(ApiEndPointConstant.TestHistory.CreateTestHistory)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateTestHistory([FromBody] CreateTestHistoryRequest request)
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

            var response = await _testHistoryService.CreateTestHistory(request);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.TestHistory.GetAllTestHistory)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllTestHistory([FromQuery] int? page, [FromQuery] int? size, [FromQuery] int? grade, [FromQuery] string? status, [FromQuery] bool? sortByGrade)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _testHistoryService.GetAllTestHistory(pageNumber, pageSize, grade, status, sortByGrade);

            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.TestHistory.GetTestHistoryById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetTestHistoryById([FromRoute] Guid id)
        {
            var response = await _testHistoryService.GetTestHistoryById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.TestHistory.GetTestHistoryByAccountId)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetTestHistoryByAccountId([FromQuery] int? page, [FromQuery] int? size, [FromQuery] int? grade, [FromQuery] string? status, [FromQuery] bool? sortByGrade)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _testHistoryService.GetTestHistoryByAccountId(pageNumber, pageSize, grade, status, sortByGrade);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpPatch(ApiEndPointConstant.TestHistory.UpdateTestHistory)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateTestHistory([FromRoute] Guid id, [FromBody] UpdateTestHistoryRequest request)
        {

            var response = await _testHistoryService.UpdateTestHistory(id, request);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpDelete(ApiEndPointConstant.TestHistory.RemoveTestHistory)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RemoveTestHistory([FromRoute] Guid id)
        {
            var response = await _testHistoryService.RemoveTestHistory(id);

            return StatusCode(int.Parse(response.status), response);
        }
    }
}
