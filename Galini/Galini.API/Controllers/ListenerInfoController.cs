
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
        
        [HttpGet(ApiEndPointConstant.ListenerInfo.GetListListenerInfo)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetListListenerInfo([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _listenerInfoService.GetAllListenerInfo(pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.ListenerInfo.GetListenerInfoByAccountId)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetListenerInfoByAccountId([FromRoute] Guid id)
        {
            var response = await _listenerInfoService.GetListenerInfoByAccountId(id);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.ListenerInfo.GetListenerInfo)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetListenerInfo([FromRoute] Guid id)
        {
            var response = await _listenerInfoService.GetListenerInfoById(id);
            return StatusCode(int.Parse(response.status), response);
        }
        
        [HttpPut(ApiEndPointConstant.ListenerInfo.UpdateListenerInfo)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateListenerInfo([FromRoute] Guid id, UpdateListenerInfoRequest request)
        {
            var response = await _listenerInfoService.UpdateListenerInfo(id, request);
            return StatusCode(int.Parse(response.status), response);
        }
        
        [HttpDelete(ApiEndPointConstant.ListenerInfo.DeleteListenerInfo)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteListenerInfo([FromRoute] Guid id)
        {
            var response = await _listenerInfoService.RemoveListenerInfo(id);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}
