
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
    public class TopicController : BaseController<TopicController>
    {
        private readonly ITopicService _service;

        public TopicController(ILogger<TopicController> logger, ITopicService service) : base(logger)
        {
            _service = service;
        }

        /// <summary>
        /// API tạo topic mới.
        /// </summary>
        /// <remarks>
        /// - Nhận dữ liệu từ client dưới dạng `CreateTopicRequest`.  
        /// - Kiểm tra tính hợp lệ của dữ liệu trước khi tạo topic.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="request">Dữ liệu topic cần tạo.</param>
        /// <returns>
        /// - `200 OK`: Tạo topic thành công.  
        /// - `400 Bad Request`: Yêu cầu không hợp lệ.  
        /// - `404 Not Found`: Không tìm thấy tài nguyên.
        /// </returns>
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

        /// <summary>
        /// API lấy danh sách topic với phân trang.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách topic có hỗ trợ phân trang.  
        /// - Nếu không truyền `page` hoặc `size`, giá trị mặc định sẽ được sử dụng (`page = 1`, `size = 10`).  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="page">Số trang hiện tại (mặc định là 1).</param>
        /// <param name="size">Số lượng topic trên mỗi trang (mặc định là 10).</param>
        /// <returns>
        /// - `200 OK`: Trả về danh sách topic thành công.
        /// </returns>
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

        /// <summary>
        /// API lấy chi tiết một topic theo ID.
        /// </summary>
        /// <remarks>
        /// - Nhận `id` của topic và trả về thông tin chi tiết.  
        /// - Nếu topic không tồn tại, trả về lỗi `404 Not Found`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của topic cần lấy.</param>
        /// <returns>
        /// - `200 OK`: Trả về thông tin topic thành công.  
        /// - `404 Not Found`: Không tìm thấy topic.
        /// </returns>
        [HttpGet(ApiEndPointConstant.Topic.GetTopic)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetTopics([FromRoute] Guid id)
        {
            var response = await _service.GetTopicById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API cập nhật thông tin topic.
        /// </summary>
        /// <remarks>
        /// - Nhận `id` của topic và dữ liệu cần cập nhật (`UpdateTopicRequest`).  
        /// - Nếu topic không tồn tại, trả về lỗi `404 Not Found`.  
        /// - Nếu dữ liệu không hợp lệ, trả về lỗi `400 Bad Request`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của topic cần cập nhật.</param>
        /// <param name="request">Dữ liệu cập nhật topic.</param>
        /// <returns>
        /// - `200 OK`: Cập nhật topic thành công.  
        /// - `404 Not Found`: Không tìm thấy topic.  
        /// - `400 Bad Request`: Dữ liệu không hợp lệ.
        /// </returns>
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

        /// <summary>
        /// API xóa một topic theo ID.
        /// </summary>
        /// <remarks>
        /// - Nhận `id` của topic cần xóa.  
        /// - Nếu topic không tồn tại, trả về lỗi `404 Not Found`.  
        /// - Nếu không thể xóa, trả về lỗi `400 Bad Request`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của topic cần xóa.</param>
        /// <returns>
        /// - `200 OK`: Xóa topic thành công.  
        /// - `404 Not Found`: Không tìm thấy topic.  
        /// - `400 Bad Request`: Không thể xóa topic.
        /// </returns>
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
