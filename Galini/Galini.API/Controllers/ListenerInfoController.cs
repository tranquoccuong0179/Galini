
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

        /// <summary>
        /// API đăng ký tài khoản mới cho người dùng.
        /// </summary>
        /// <remarks>
        /// - Nhận thông tin đăng ký từ `CreateListenerInfoModel`, bao gồm RegisterUserRequest và CreateListenerInfoRequest.
        /// - Trả về `BaseResponse` chứa kết quả đăng ký.
        /// </remarks>
        /// <param name="request">Dữ liệu đăng ký tài khoản.</param>
        /// <returns>
        /// - `200 OK`: Đăng ký thành công.  
        /// - `400 Bad Request`: Dữ liệu đầu vào không hợp lệ.
        /// </returns>
        [HttpPost(ApiEndPointConstant.ListenerInfo.CreateListenerInfo)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateNewListener([FromBody] CreateListenerInfoModel request)
        {
            var response = await _listenerInfoService.CreateListenerInfo(request.RegisterRequest, request.ListenerRequest);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách thông tin người nghe với phân trang, hỗ trợ lọc và sắp xếp.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách thông tin người nghe có hỗ trợ phân trang.  
        /// - Nếu không truyền `page` hoặc `size`, giá trị mặc định sẽ được sử dụng (`page = 1`, `size = 10`).  
        /// - Có thể lọc theo tên và sắp xếp theo tên, giá hoặc số sao.  
        /// - Khi `sortByName` là `true`, sắp xếp tên theo thứ tự tăng dần, `false` là giảm dần.  
        /// - Khi `sortByPrice` là `true`, sắp xếp giá theo thứ tự tăng dần, `false` là giảm dần.  
        /// - Khi `sortByStar` là `true`, sắp xếp số sao theo thứ tự tăng dần, `false` là giảm dần.
        /// - Khi không có bất kì bộ lọc nào thì hệ sẽ sắp xếp theo thời gian tạo giảm dần
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="page">Số trang hiện tại (mặc định là 1).</param>
        /// <param name="size">Số lượng người nghe trên mỗi trang (mặc định là 10).</param>
        /// <param name="name">Lọc danh sách theo tên.</param>
        /// <param name="sortByName">Sắp xếp theo tên (`true`: tăng dần, `false`: giảm dần).</param>
        /// <param name="sortByPrice">Sắp xếp theo giá (`true`: tăng dần, `false`: giảm dần).</param>
        /// <param name="sortByStar">Sắp xếp theo số sao (`true`: tăng dần, `false`: giảm dần).</param>
        /// <returns>
        /// - `200 OK`: Trả về danh sách thông tin người nghe thành công.  
        /// </returns>
        [HttpGet(ApiEndPointConstant.ListenerInfo.GetListListenerInfo)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetListListenerInfo([FromQuery] int? page, 
                                                             [FromQuery] int? size, 
                                                             [FromQuery] string? name, 
                                                             [FromQuery] bool? sortByName, 
                                                             [FromQuery] bool? sortByPrice, 
                                                             [FromQuery] bool? sortByStar)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _listenerInfoService.GetAllListenerInfo(pageNumber, pageSize, name, sortByName, sortByPrice, sortByStar);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy thông tin người nghe theo AccountId.
        /// </summary>
        /// <remarks>
        /// - Trả về thông tin chi tiết của người nghe dựa trên `id` của tài khoản.  
        /// - Nếu không tìm thấy thông tin, trả về lỗi `404 Not Found`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của tài khoản người nghe.</param>
        /// <returns>
        /// - `200 OK`: Trả về thông tin người nghe thành công.  
        /// - `404 Not Found`: Không tìm thấy thông tin người nghe.
        /// </returns>
        [HttpGet(ApiEndPointConstant.ListenerInfo.GetListenerInfoByAccountId)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetListenerInfoByAccountId([FromRoute] Guid id)
        {
            var response = await _listenerInfoService.GetListenerInfoByAccountId(id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy thông tin người nghe theo ID.
        /// </summary>
        /// <remarks>
        /// - Trả về thông tin chi tiết của người nghe dựa trên `id`.  
        /// - Nếu không tìm thấy thông tin, trả về lỗi `404 Not Found`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của người nghe.</param>
        /// <returns>
        /// - `200 OK`: Trả về thông tin người nghe thành công.  
        /// - `404 Not Found`: Không tìm thấy thông tin người nghe.
        /// </returns>
        [HttpGet(ApiEndPointConstant.ListenerInfo.GetListenerInfo)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetListenerInfo([FromRoute] Guid id)
        {
            var response = await _listenerInfoService.GetListenerInfoById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API cập nhật thông tin người nghe theo ID.
        /// </summary>
        /// <remarks>
        /// - Cập nhật thông tin người nghe dựa trên `id` được cung cấp.  
        /// - Yêu cầu phải có dữ liệu hợp lệ trong `request`.  
        /// - Nếu `id` không tồn tại trong hệ thống, trả về lỗi `404 Not Found`.  
        /// - Nếu dữ liệu cập nhật không hợp lệ, trả về lỗi `400 Bad Request`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của người nghe cần cập nhật.</param>
        /// <param name="request">Thông tin cập nhật cho người nghe.</param>
        /// <returns>
        /// - `200 OK`: Cập nhật thành công.  
        /// - `400 Bad Request`: Yêu cầu không hợp lệ hoặc dữ liệu không hợp lệ.  
        /// - `404 Not Found`: Không tìm thấy thông tin người nghe.
        /// </returns>
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

        /// <summary>
        /// API xoá thông tin người nghe.
        /// </summary>
        /// <remarks>
        /// - Xoá thông tin người nghe dựa trên `id`.  
        /// - Nếu không tìm thấy thông tin, trả về lỗi `404 Not Found`.  
        /// - Nếu không thể xoá, trả về lỗi `400 Bad Request`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của người nghe cần xoá.</param>
        /// <returns>
        /// - `200 OK`: Xoá thông tin thành công.  
        /// - `400 Bad Request`: Không thể xoá thông tin.  
        /// - `404 Not Found`: Không tìm thấy thông tin người nghe.
        /// </returns>
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
