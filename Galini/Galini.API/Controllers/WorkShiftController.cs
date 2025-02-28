using Galini.API.Constants;
using Galini.Models.Payload.Request.WorkShift;
using Galini.Models.Payload.Response;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class WorkShiftController : BaseController<WorkShiftController>
    {
        private readonly IWorkShiftService _workShiftService;
        public WorkShiftController(ILogger<WorkShiftController> logger, IWorkShiftService workShiftService) : base(logger)
        {
            _workShiftService = workShiftService;
        }

        [HttpPost(ApiEndPointConstant.WorkShift.CreateWorkShift)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateWorkShift([FromBody] CreateWorkShiftRequest request, [FromRoute] Guid id)
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

            var response = await _workShiftService.CreateWorkShift(request, id);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.WorkShift.GetAllWorkShift)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllWorkShift([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _workShiftService.GetAllWorkShift(pageNumber, pageSize);

            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.WorkShift.GetWorkShiftById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetWorkShiftById([FromRoute] Guid id)
        {
            var response = await _workShiftService.GetWorkShiftById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.WorkShift.GetWorkShiftByAccountId)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetWorkShiftByAccountId([FromQuery] int? page, [FromQuery] int? size, [FromRoute] Guid id)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _workShiftService.GetWorkShiftByAccountId(pageNumber, pageSize, id);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpPatch(ApiEndPointConstant.WorkShift.UpdateWorkShift)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateWorkShift([FromRoute] Guid id, [FromBody] UpdateWorkShiftRequest request)
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
            var response = await _workShiftService.UpdateWorkShift(request, id);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpDelete(ApiEndPointConstant.WorkShift.RemoveWorkShift)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RemoveWorkShift([FromRoute] Guid id)
        {
            var response = await _workShiftService.RemoveWorkShift(id);

            return StatusCode(int.Parse(response.status), response);
        }
    }
}
