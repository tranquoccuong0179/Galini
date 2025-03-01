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

        /// <summary>
        /// API tạo câu hỏi mới.
        /// </summary>
        /// <remarks>
        /// - Nhận dữ liệu từ client dưới dạng `CreateQuestionRequest`.  
        /// - Kiểm tra tính hợp lệ của dữ liệu trước khi tạo câu hỏi.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="request">Dữ liệu câu hỏi cần tạo.</param>
        /// <returns>
        /// - `200 OK`: Tạo câu hỏi thành công.  
        /// - `400 Bad Request`: Yêu cầu không hợp lệ.
        /// </returns>
        [HttpPost(ApiEndPointConstant.Question.CreateQuestion)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionRequest request)
        {
            var response = await _questionService.CreateQuestion(request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách câu hỏi với phân trang, hỗ trợ lọc và sắp xếp.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách câu hỏi có hỗ trợ phân trang.  
        /// - Nếu không truyền `page` hoặc `size`, giá trị mặc định sẽ được sử dụng (`page = 1`, `size = 10`).  
        /// - Có thể lọc theo nội dung (`content`).  
        /// - Khi `sortByContent` là `true`, sắp xếp nội dung theo thứ tự tăng dần, `false` là giảm dần.  
        /// - Khi không có bộ lọc nào, danh sách sẽ được sắp xếp theo thời gian tạo giảm dần.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="page">Số trang hiện tại (mặc định là 1).</param>
        /// <param name="size">Số lượng câu hỏi trên mỗi trang (mặc định là 10).</param>
        /// <param name="content">Tìm kiếm câu hỏi theo nội dung.</param>
        /// <param name="sortByContent">Sắp xếp theo nội dung (`true`: tăng dần, `false`: giảm dần).</param>
        /// <returns>
        /// - `200 OK`: Trả về danh sách câu hỏi thành công.
        /// </returns>
        [HttpGet(ApiEndPointConstant.Question.GetAllQuestion)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetNotifications([FromQuery] int? page,
                                                          [FromQuery] int? size,
                                                          [FromQuery] string? content,
                                                          [FromQuery] bool? sortByContent)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _questionService.GetAllQuestion(pageNumber, pageSize, content, sortByContent);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy chi tiết một câu hỏi theo ID.
        /// </summary>
        /// <remarks>
        /// - Nhận `id` của câu hỏi và trả về thông tin chi tiết.
        /// - Nếu câu hỏi không tồn tại, trả về lỗi `404 Not Found`.
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của câu hỏi cần lấy.</param>
        /// <returns>
        /// - `200 OK`: Trả về thông tin câu hỏi thành công.  
        /// - `404 Not Found`: Không tìm thấy câu hỏi.
        /// </returns>
        [HttpGet(ApiEndPointConstant.Question.GetQuestionById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetQuestionById([FromRoute] Guid id)
        {
            var response = await _questionService.GetQuestionById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API xóa một câu hỏi theo ID.
        /// </summary>
        /// <remarks>
        /// - Nhận `id` của câu hỏi cần xóa.  
        /// - Nếu câu hỏi không tồn tại, trả về lỗi `404 Not Found`.  
        /// - Nếu không thể xóa, trả về lỗi `400 Bad Request`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của câu hỏi cần xóa.</param>
        /// <returns>
        /// - `200 OK`: Xóa câu hỏi thành công.  
        /// - `404 Not Found`: Không tìm thấy câu hỏi.  
        /// - `400 Bad Request`: Không thể xóa câu hỏi.
        /// </returns>
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

        /// <summary>
        /// API cập nhật thông tin câu hỏi.
        /// </summary>
        /// <remarks>
        /// - Nhận `id` của câu hỏi và dữ liệu cần cập nhật (`UpdateQuestionRequest`).  
        /// - Nếu câu hỏi không tồn tại, trả về lỗi `404 Not Found`.  
        /// - Nếu dữ liệu không hợp lệ, trả về lỗi `400 Bad Request`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của câu hỏi cần cập nhật.</param>
        /// <param name="request">Dữ liệu cập nhật câu hỏi.</param>
        /// <returns>
        /// - `200 OK`: Cập nhật câu hỏi thành công.  
        /// - `404 Not Found`: Không tìm thấy câu hỏi.  
        /// - `400 Bad Request`: Dữ liệu không hợp lệ.
        /// </returns>
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
