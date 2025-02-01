
using Galini.API.Constants;
using Galini.Models.Payload.Request.ListenerInfo;
using Galini.Models.Payload.Request.User;
using Galini.Models.Payload.Response;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class ListenerInfoController : BaseController<ListenerInfoController>
    {
        private readonly IListenerInfoService _listenerInfoService;
        public ListenerInfoController(ILogger<ListenerInfoController> logger, IListenerInfoService listenerInfoService) : base(logger)
        {
            _listenerInfoService = listenerInfoService;
        }

        [HttpPost(ApiEndPointConstant.ListenerInfo.CreateListenerInfo)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateNewListener([FromBody] CreateListenerInfoModel request)
        {
            var response = await _listenerInfoService.CreateListenerInfo(request.RegisterRequest, request.ListenerRequest);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}
