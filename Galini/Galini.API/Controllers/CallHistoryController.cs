using Galini.API.Constants;
using Galini.Models.Payload.Request.CallHistory;
using Galini.Models.Payload.Request.Premium;
using Galini.Models.Payload.Response;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class CallHistoryController : BaseController<CallHistoryController>
    {
        private readonly ICallHistoryService _callHistoryService;
        public CallHistoryController(ILogger<CallHistoryController> logger, ICallHistoryService callHistoryService) : base(logger)
        {
            _callHistoryService = callHistoryService;
        }

        /// <summary>
        /// API tạo lịch sử cuộc gọi ngay khi cuộc gọi kết thúc.
        /// </summary>
        /// <remarks>
        /// - Nhận thông tin cuộc gọi (`CreateCallHistoryRequest`).  
        /// - Trả về `BaseResponse` chứa thông tin lịch sử cuộc gọi vừa tạo.  
        /// - Nếu dữ liệu đầu vào không hợp lệ, trả về lỗi `400 Bad Request`.  
        /// </remarks>
        /// <param name="request">Thông tin lịch sử cuộc gọi cần tạo.</param>
        /// <returns>
        /// - `200 OK`: Tạo lịch sử cuộc gọi thành công.  
        /// - `400 Bad Request`: Dữ liệu không hợp lệ.  
        /// </returns>
        [HttpPost(ApiEndPointConstant.CallHistory.CreateCallHistory)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateCallHistory([FromBody] CreateCallHistoryRequest request)
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

            var response = await _callHistoryService.CreateCallHistory(request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách lịch sử cuộc gọi với phân trang, hỗ trợ lọc và sắp xếp.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách lịch sử cuộc gọi có hỗ trợ phân trang.  
        /// - Nếu không truyền `page` hoặc `size`, giá trị mặc định sẽ được sử dụng (`page = 1`, `size = 10`).  
        /// - Có thể lọc theo thời gian bắt đầu (`timeStart`), thời gian kết thúc (`timeEnd`), thời lượng cuộc gọi (`duration`), và cuộc gọi nhỡ (`isMissCall`).  
        /// - Khi `sortByTimeStart` là `true`, sắp xếp thời gian bắt đầu theo thứ tự tăng dần, `false` là giảm dần.  
        /// - Khi `sortByTimeEnd` là `true`, sắp xếp thời gian kết thúc theo thứ tự tăng dần, `false` là giảm dần.  
        /// - Khi `sortByDuration` là `true`, sắp xếp thời lượng cuộc gọi theo thứ tự tăng dần, `false` là giảm dần.  
        /// - Khi `sortByMissCall` là `true`, sắp xếp cuộc gọi nhỡ theo thứ tự tăng dần, `false` là giảm dần.  
        /// - Khi không có bất kỳ bộ lọc nào, hệ thống sẽ sắp xếp theo thời gian tạo giảm dần.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.  
        /// </remarks>
        /// <param name="page">Số trang hiện tại (mặc định là 1).</param>
        /// <param name="size">Số lượng lịch sử cuộc gọi trên mỗi trang (mặc định là 10).</param>
        /// <param name="timeStart">Lọc danh sách theo thời gian bắt đầu.</param>
        /// <param name="timeEnd">Lọc danh sách theo thời gian kết thúc.</param>
        /// <param name="duration">Lọc danh sách theo thời lượng cuộc gọi.</param>
        /// <param name="isMissCall">Lọc danh sách theo cuộc gọi nhỡ (`true`: chỉ lấy cuộc gọi nhỡ, `false`: chỉ lấy cuộc gọi có nhận).</param>
        /// <param name="sortByTimeStart">Sắp xếp theo thời gian bắt đầu (`true`: tăng dần, `false`: giảm dần).</param>
        /// <param name="sortByTimeEnd">Sắp xếp theo thời gian kết thúc (`true`: tăng dần, `false`: giảm dần).</param>
        /// <param name="sortByDuration">Sắp xếp theo thời lượng cuộc gọi (`true`: tăng dần, `false`: giảm dần).</param>
        /// <param name="sortByMissCall">Sắp xếp theo trạng thái cuộc gọi nhỡ (`true`: tăng dần, `false`: giảm dần).</param>
        /// <returns>
        /// - `200 OK`: Trả về danh sách lịch sử cuộc gọi thành công.  
        /// - `400 Bad Request`: Dữ liệu không hợp lệ.  
        /// </returns>
        [HttpGet(ApiEndPointConstant.CallHistory.GetAllCallHistory)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllCallHistory([FromQuery] int? page, [FromQuery] int? size, 
                                                           [FromQuery] DateTime? timeStart, [FromQuery] DateTime? timeEnd, 
                                                           [FromQuery] int? duration, [FromQuery] bool? isMissCall, 
                                                           [FromQuery] bool? sortByTimeStart, [FromQuery] bool? sortByTimeEnd,                                  
                                                           [FromQuery] bool? sortByDuration, [FromQuery] bool? sortByMissCall)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _callHistoryService.GetAllCallHistory(pageNumber, pageSize, timeStart, timeEnd, duration, isMissCall, sortByTimeStart, sortByTimeEnd, sortByDuration, sortByMissCall);

            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy thông tin lịch sử cuộc gọi theo ID.
        /// </summary>
        /// <remarks>
        /// - Trả về thông tin chi tiết của lịch sử cuộc gọi dựa trên `id`.  
        /// - Nếu không tìm thấy thông tin, trả về lỗi `404 Not Found`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.  
        /// </remarks>
        /// <param name="id">ID của lịch sử cuộc gọi.</param>
        /// <returns>
        /// - `200 OK`: Trả về thông tin lịch sử cuộc gọi thành công.  
        /// - `404 Not Found`: Không tìm thấy lịch sử cuộc gọi.  
        /// </returns>
        [HttpGet(ApiEndPointConstant.CallHistory.GetCallHistoryById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetCallHistoryById([FromRoute] Guid id)
        {
            var response = await _callHistoryService.GetCallHistoryById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API cập nhật thông tin lịch sử cuộc gọi theo ID.
        /// </summary>
        /// <remarks>
        /// - Cập nhật thông tin lịch sử cuộc gọi dựa trên `id` được cung cấp.  
        /// - Yêu cầu phải có dữ liệu hợp lệ trong `request`.  
        /// - Nếu `id` không tồn tại trong hệ thống, trả về lỗi `404 Not Found`.  
        /// - Nếu dữ liệu cập nhật không hợp lệ, trả về lỗi `400 Bad Request`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.  
        /// </remarks>
        /// <param name="id">ID của lịch sử cuộc gọi cần cập nhật.</param>
        /// <param name="request">Thông tin cập nhật cho lịch sử cuộc gọi.</param>
        /// <returns>
        /// - `200 OK`: Cập nhật thành công.  
        /// - `400 Bad Request`: Yêu cầu không hợp lệ hoặc dữ liệu không hợp lệ.  
        /// - `404 Not Found`: Không tìm thấy lịch sử cuộc gọi.  
        /// </returns>
        [HttpPatch(ApiEndPointConstant.CallHistory.UpdateCallHistory)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateCallHistory([FromRoute] Guid id, [FromBody] UpdateCallHistoryRequest request)
        {

            var response = await _callHistoryService.UpdateCallHistory(id, request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API xoá thông tin lịch sử cuộc gọi.
        /// </summary>
        /// <remarks>
        /// - Xoá thông tin lịch sử cuộc gọi dựa trên `id`.  
        /// - Nếu không tìm thấy thông tin, trả về lỗi `404 Not Found`.  
        /// - Nếu không thể xoá, trả về lỗi `400 Bad Request`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.  
        /// </remarks>
        /// <param name="id">ID của lịch sử cuộc gọi cần xoá.</param>
        /// <returns>
        /// - `200 OK`: Xoá thông tin thành công.  
        /// - `400 Bad Request`: Không thể xoá thông tin.  
        /// - `404 Not Found`: Không tìm thấy lịch sử cuộc gọi.  
        /// </returns>
        [HttpDelete(ApiEndPointConstant.CallHistory.RemoveCallHistory)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RemoveCallHistory([FromRoute] Guid id)
        {
            var response = await _callHistoryService.RemoveCallHistory(id);

            return StatusCode(int.Parse(response.status), response);
        }
    }
}
