using Galini.API.Constants;
using Galini.Models.Payload.Request.CallHistory;
using Galini.Models.Payload.Request.Message;
using Galini.Models.Payload.Response;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class MessageController : BaseController<MessageController>
    {
        private readonly IMessageService _messageService;

        public MessageController(ILogger<MessageController> logger, IMessageService messageService) : base(logger)
        {
            _messageService = messageService;
        }

        /// <summary>
        /// API tạo tin nhắn mới trong phòng chat, mỗi khi người dùng gửi tin nhắn thì gọi API này.
        /// </summary>
        /// <remarks>
        /// - Xác minh phòng chat (`DirectChat`) có tồn tại hay không.  
        /// - Xác minh người gửi (`Sender`) có hợp lệ hay không.  
        /// - Tạo tin nhắn và lưu vào database.  
        /// - Trả về `200 OK` nếu thành công, `400 Bad Request` nếu thất bại, hoặc `404 Not Found` nếu không tìm thấy dữ liệu.
        /// - Yêu cầu thông tin người dùng thông qua `CreateMessageRequest`.
        /// - Trả về `BaseResponse` chứa kết quả đăng ký.
        /// </remarks>
        /// <param name="request">Dữ liệu tin nhắn cần tạo.</param>
        /// <param name="id">ID của phòng chat.</param>
        /// <returns>
        /// - `200 OK`: Tạo tin nhắn thành công.  
        /// - `400 Bad Request`: Dữ liệu không hợp lệ hoặc lưu thất bại.  
        /// - `404 Not Found`: Không tìm thấy phòng chat hoặc tài khoản.
        /// </returns>
        [HttpPost(ApiEndPointConstant.Message.CreateMessage)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateMessage([FromBody] CreateMessageRequest request, [FromQuery] Guid id)
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

            var response = await _messageService.CreateMessage(request, id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API tạo tin nhắn kiểu "Call" trong phòng chat. Gọi API này khi người dùng kết thúc cuộc gọi thì gửi tin nhắn này.
        /// </summary>
        /// <remarks>
        /// - Kiểm tra phòng chat (`DirectChat`) có tồn tại và đang hoạt động hay không.
        /// - Xác thực tài khoản người gửi (`Sender`) có hợp lệ và đang hoạt động hay không.
        /// - Tạo tin nhắn kiểu "Call", lưu vào database và trả về kết quả.
        /// - Trả về `200 OK` nếu thành công, `400 Bad Request` nếu dữ liệu không hợp lệ hoặc lưu thất bại, `404 Not Found` nếu phòng chat/tài khoản không tồn tại.
        /// - Nhận dữ liệu từ `CreateMessageRequest` và trả về `BaseResponse` chứa kết quả.
        /// </remarks>
        /// <param name="request">Dữ liệu tin nhắn cần tạo.</param>
        /// <param name="id">ID của phòng chat (DirectChat).</param>
        /// <returns>
        /// - `200 OK`: Tin nhắn được tạo thành công.
        /// - `400 Bad Request`: Dữ liệu không hợp lệ hoặc tạo tin nhắn thất bại.
        /// - `404 Not Found`: Không tìm thấy phòng chat hoặc tài khoản.
        /// </returns>
        [HttpPost(ApiEndPointConstant.Message.CreateMessageCall)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateMessageCall([FromBody] CreateMessageRequest request, [FromQuery] Guid id)
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

            var response = await _messageService.CreateMessageCall(request, id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách tin nhắn với phân trang.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách tin nhắn đang hoạt động (`IsActive = true`) với hỗ trợ phân trang.
        /// - Nếu không truyền `page` hoặc `size`, giá trị mặc định là `page = 1`, `size = 10`.
        /// - Kiểm tra `page` và `size` phải lớn hơn 0, nếu không sẽ trả về lỗi.
        /// - Không hỗ trợ lọc hoặc sắp xếp tùy chỉnh, mặc định sắp xếp theo thứ tự mặc định của repository.
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="page">Số trang hiện tại (mặc định là 1).</param>
        /// <param name="size">Số lượng tin nhắn trên mỗi trang (mặc định là 10).</param>
        /// <returns>
        /// - `200 OK`: Trả về danh sách tin nhắn thành công.
        /// - `400 Bad Request`: Giá trị `page` hoặc `size` không hợp lệ (nhỏ hơn 1).
        /// </returns>
        [HttpGet(ApiEndPointConstant.Message.GetAllMessage)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllMessage([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _messageService.GetAllMessage(pageNumber, pageSize);

            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách tin nhắn trong phòng chat theo DirectChatId.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách 20 tin nhắn gần nhất trước thời điểm (`beforeCursor`) tin nhắn đang hoạt động trong phòng chat dựa trên `id` của phòng chat.
        /// - Hỗ trợ lọc tin nhắn trước một thời điểm cụ thể (`beforeCursor`), nếu không truyền thì lấy 20 tin gần nhất.
        /// - Sắp xếp tin nhắn theo thời gian tạo giảm dần và giới hạn tối đa 20 tin nhắn.
        /// - Nếu không tìm thấy phòng chat, trả về lỗi `404 Not Found`.
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của phòng chat (DirectChat).</param>
        /// <param name="beforeCursor">Thời điểm để lọc tin nhắn trước đó (tuỳ chọn).</param>
        /// <returns>
        /// - `200 OK`: Trả về danh sách tin nhắn thành công.
        /// - `404 Not Found`: Không tìm thấy phòng chat.
        /// </returns>
        [HttpGet(ApiEndPointConstant.Message.GetMessageByDirectChatId)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetMessageByDirectChatId([FromRoute] Guid id, [FromQuery] DateTime? beforeCursor)
        {
            var response = await _messageService.GetMessageByDirectChatId(id, beforeCursor);

            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy thông tin tin nhắn theo ID.
        /// </summary>
        /// <remarks>
        /// - Trả về chi tiết tin nhắn dựa trên `id` của nó.  
        /// - Chỉ lấy tin nhắn còn hoạt động (`IsActive = true`).  
        /// - Nếu không tìm thấy tin nhắn, trả về lỗi `404 Not Found`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của tin nhắn.</param>
        /// <returns>
        /// - `200 OK`: Lấy được thông tin tin nhắn thành công.  
        /// - `404 Not Found`: Không tìm thấy tin nhắn với ID này.
        /// </returns>
        [HttpGet(ApiEndPointConstant.Message.GetMessageById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetMessageById([FromRoute] Guid id)
        {
            var response = await _messageService.GetMessageById(id);

            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API cập nhật thông tin tin nhắn theo ID.
        /// </summary>
        /// <remarks>
        /// - Cập nhật tin nhắn dựa trên `id` được truyền vào.  
        /// - Cần dữ liệu hợp lệ trong `request`, nếu không sẽ lỗi.  
        /// - Nếu `id` không tồn tại, trả về lỗi `404 Not Found`.  
        /// - Nếu dữ liệu không hợp lệ hoặc cập nhật thất bại, trả về lỗi `400 Bad Request`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của tin nhắn cần cập nhật.</param>
        /// <param name="request">Thông tin cập nhật cho tin nhắn.</param>
        /// <returns>
        /// - `200 OK`: Cập nhật tin nhắn thành công.  
        /// - `400 Bad Request`: Dữ liệu không hợp lệ hoặc cập nhật thất bại.  
        /// - `404 Not Found`: Không tìm thấy tin nhắn.
        /// </returns>
        [HttpPatch(ApiEndPointConstant.Message.UpdateMessage)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateMessage([FromRoute] Guid id, [FromBody] UpdateMessageRequest request)
        {

            var response = await _messageService.UpdateMessage(id, request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API xoá tin nhắn theo ID.
        /// </summary>
        /// <remarks>
        /// - Xoá tin nhắn dựa trên `id` được cung cấp.  
        /// - Nếu không tìm thấy tin nhắn, trả về lỗi `404 Not Found`.  
        /// - Nếu không xoá được (do lỗi hệ thống hoặc logic), trả về lỗi `400 Bad Request`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của tin nhắn cần xoá.</param>
        /// <returns>
        /// - `200 OK`: Xoá tin nhắn thành công.  
        /// - `400 Bad Request`: Không thể xoá tin nhắn.  
        /// - `404 Not Found`: Không tìm thấy tin nhắn.
        /// </returns>
        [HttpDelete(ApiEndPointConstant.Message.RemoveMessage)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RemoveMessage([FromRoute] Guid id)
        {
            var response = await _messageService.RemoveMessage(id);

            return StatusCode(int.Parse(response.status), response);
        }
    }
}
