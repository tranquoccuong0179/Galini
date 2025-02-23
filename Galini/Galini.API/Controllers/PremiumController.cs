using Galini.API.Constants;
using Galini.Models.Payload.Request.Premium;
using Galini.Models.Payload.Request.UserInfo;
using Galini.Models.Payload.Response;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class PremiumController : BaseController<PremiumController>
    {
        private readonly IPremiumService _premiumService;
        public PremiumController(ILogger<PremiumController> logger, IPremiumService premiumService) : base(logger)
        {
            _premiumService = premiumService;
        }

        [HttpPost(ApiEndPointConstant.Premium.CreatePremium)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreatePremium([FromBody] CreatePremiumRequest request)
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

            var response = await _premiumService.CreatePremium(request);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.Premium.GetAllPremium)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllPremium([FromQuery] int? page, [FromQuery] int? size,
                                                                            [FromQuery] int? friend,
                                                                            [FromQuery] bool? timelimit,
                                                                            [FromQuery] int? match,
                                                                            [FromQuery] double? minPrice,
                                                                            [FromQuery] double? maxPrice,
                                                                            [FromQuery] bool? sortByFriend,
                                                                            [FromQuery] bool? sortByMatch,
                                                                            [FromQuery] bool? sortByPrice,
                                                                            [FromQuery] bool? sortByTimelimit)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _premiumService.GetAllPremium(pageNumber, pageSize, friend, timelimit, match, minPrice, maxPrice, sortByFriend, sortByMatch, sortByPrice, sortByTimelimit);

            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.Premium.GetPremiumById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetPremiumById([FromRoute] Guid id)
        {
            var response = await _premiumService.GetPremiumById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpPatch(ApiEndPointConstant.Premium.UpdatePremium)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateUserInfo([FromRoute] Guid id, [FromBody] UpdatePremiumRequest request)
        {

            var response = await _premiumService.UpdatePremium(id, request);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpDelete(ApiEndPointConstant.Premium.RemovePremium)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RemovePremium([FromRoute] Guid id)
        {
            var response = await _premiumService.RemovePremium(id);

            return StatusCode(int.Parse(response.status), response);
        }
    }
}
