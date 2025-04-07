
using Galini.API.Constants;
using Galini.Models.Payload.Request.DirectChat;
using Galini.Models.Payload.Response;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class DirectChatController : BaseController<DirectChatController>
    {
        private readonly IDirectChatService _directChat;
        public DirectChatController(ILogger<DirectChatController> logger, IDirectChatService directChat) : base(logger)
        {
            _directChat = directChat;
        }

        /// <summary>
        /// API tạo cuộc trò chuyện trực tiếp.
        /// </summary>
        /// <remarks>
        /// - Nhận dữ liệu từ client dưới dạng `CreateDirectChatRequest`.  
        /// - Kiểm tra tính hợp lệ của dữ liệu trước khi tạo cuộc trò chuyện.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="request">Dữ liệu cuộc trò chuyện cần tạo.</param>
        /// <returns>
        /// - `200 OK`: Tạo cuộc trò chuyện thành công.  
        /// - `400 Bad Request`: Yêu cầu không hợp lệ.
        /// </returns>
        [HttpPost(ApiEndPointConstant.DirectChat.CreateDirectChat)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateDirectChat([FromBody] CreateDirectChatRequest request)
        {
            var response = await _directChat.CreateDirectChat(request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách các cuộc trò chuyện trực tiếp với phân trang.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách cuộc trò chuyện có hỗ trợ phân trang.  
        /// - Nếu không truyền `page` hoặc `size`, giá trị mặc định sẽ được sử dụng (`page = 1`, `size = 10`).  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="page">Số trang hiện tại (mặc định là 1).</param>
        /// <param name="size">Số lượng cuộc trò chuyện trên mỗi trang (mặc định là 10).</param>
        /// <returns>
        /// - `200 OK`: Trả về danh sách cuộc trò chuyện thành công.
        /// </returns>
        [HttpGet(ApiEndPointConstant.DirectChat.GetAllDirectChats)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllDirectChats([FromQuery] int? page, 
                                                           [FromQuery] int? size,
                                                           [FromQuery] string? name)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _directChat.GetAllDirectChat(pageNumber, pageSize, name);

            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách tất cả các cuộc trò chuyện trực tiếp của người dùng.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách các cuộc trò chuyện trực tiếp (direct chat) mà người dùng hiện tại tham gia.  
        /// - Mỗi phần tử bao gồm thông tin tên cuộc trò chuyện, tin nhắn mới nhất và thông tin bạn chat.  
        /// - Chỉ những cuộc trò chuyện có sự tham gia của người dùng đang đăng nhập mới được trả về.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <returns>
        /// - `200 OK`: Lấy danh sách direct chat thành công.  
        /// - `404 Not Found`: Không tìm thấy tài khoản hoặc không có cuộc trò chuyện nào.
        /// </returns>
        [HttpGet(ApiEndPointConstant.DirectChat.GetAllDirectChatUser)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllDirectChatUser()
        {
            var response = await _directChat.GetAllDirectChatUser();

            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy chi tiết một cuộc trò chuyện trực tiếp theo ID.
        /// </summary>
        /// <remarks>
        /// - Nhận `id` của cuộc trò chuyện và trả về thông tin chi tiết.  
        /// - Nếu cuộc trò chuyện không tồn tại, trả về lỗi `404 Not Found`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của cuộc trò chuyện cần lấy.</param>
        /// <returns>
        /// - `200 OK`: Trả về thông tin cuộc trò chuyện thành công.  
        /// - `404 Not Found`: Không tìm thấy cuộc trò chuyện.
        /// </returns>
        [HttpGet(ApiEndPointConstant.DirectChat.GetDirectChatById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetDirectChatById([FromRoute] Guid id)
        {
            var response = await _directChat.GetDirectChatById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API cập nhật thông tin cuộc trò chuyện trực tiếp.
        /// </summary>
        /// <remarks>
        /// - Nhận `id` của cuộc trò chuyện và dữ liệu cần cập nhật (`UpdateDirectChatRequest`).  
        /// - Nếu cuộc trò chuyện không tồn tại, trả về lỗi `404 Not Found`.  
        /// - Nếu dữ liệu không hợp lệ, trả về lỗi `400 Bad Request`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của cuộc trò chuyện cần cập nhật.</param>
        /// <param name="request">Dữ liệu cập nhật cuộc trò chuyện.</param>
        /// <returns>
        /// - `200 OK`: Cập nhật cuộc trò chuyện thành công.  
        /// - `404 Not Found`: Không tìm thấy cuộc trò chuyện.  
        /// - `400 Bad Request`: Dữ liệu không hợp lệ.
        /// </returns>
        [HttpPatch(ApiEndPointConstant.DirectChat.UpdateDirectChat)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateDirectChat([FromRoute] Guid id, [FromBody] UpdateDirectChatRequest request)
        {

            var response = await _directChat.UpdateDirectChat(id, request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API xóa một cuộc trò chuyện trực tiếp theo ID.
        /// </summary>
        /// <remarks>
        /// - Nhận `id` của cuộc trò chuyện cần xóa.  
        /// - Nếu cuộc trò chuyện không tồn tại, trả về lỗi `404 Not Found`.  
        /// - Nếu không thể xóa, trả về lỗi `400 Bad Request`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của cuộc trò chuyện cần xóa.</param>
        /// <returns>
        /// - `200 OK`: Xóa cuộc trò chuyện thành công.  
        /// - `404 Not Found`: Không tìm thấy cuộc trò chuyện.  
        /// - `400 Bad Request`: Không thể xóa cuộc trò chuyện.
        /// </returns>
        [HttpDelete(ApiEndPointConstant.DirectChat.RemoveDirectChat)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RemoveDirectChat([FromRoute] Guid id)
        {
            var response = await _directChat.RemoveDirectChat(id);

            return StatusCode(int.Parse(response.status), response);
        }
    }
}
