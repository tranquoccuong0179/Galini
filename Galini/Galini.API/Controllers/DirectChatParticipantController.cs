
using System.Drawing;
using Galini.API.Constants;
using Galini.Models.Payload.Request.DirectChat;
using Galini.Models.Payload.Request.DirectChatParticipant;
using Galini.Models.Payload.Response;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class DirectChatParticipantController : BaseController<DirectChatParticipantController>
    {
        private readonly IDirectChatParticipantService _directChatParticipantService;
        public DirectChatParticipantController(ILogger<DirectChatParticipantController> logger, IDirectChatParticipantService directChatParticipantService) : base(logger)
        {
            _directChatParticipantService = directChatParticipantService;
        }

        /// <summary>
        /// API tạo cuộc trò chuyện trực tiếp.
        /// </summary>
        /// <remarks>
        /// - Nhận dữ liệu từ client dưới dạng `CreateDirectChatParticipant`.  
        /// - Kiểm tra tính hợp lệ của dữ liệu trước khi tạo.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="request">Dữ liệu cuộc trò chuyện cần tạo.</param>
        /// <returns>
        /// - `200 OK`: Tạo cuộc trò chuyện thành công.  
        /// - `400 Bad Request`: Yêu cầu không hợp lệ.
        /// - `404 Not Found`: Không tìm thấy người dùng hoặc cuộc trò chuyện.
        /// </returns>
        [HttpPost(ApiEndPointConstant.DirectChatParticipant.CreateDirectChatParticipant)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateDirectChatParticipant([FromBody] CreateDirectChatParticipant request)
        {
            var response = await _directChatParticipantService.CreateDirectChatParticipant(request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách với phân trang.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách có hỗ trợ phân trang.  
        /// - Nếu không truyền `page` hoặc `size`, giá trị mặc định sẽ được sử dụng (`page = 1`, `size = 10`).  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="page">Số trang hiện tại (mặc định là 1).</param>
        /// <param name="size">Số lượng cuộc trò chuyện trên mỗi trang (mặc định là 10).</param>
        /// <returns>
        /// - `200 OK`: Trả về danh sách thành công.
        /// </returns>
        [HttpGet(ApiEndPointConstant.DirectChatParticipant.GetAllDirectChatParticipants)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllDirectChatParticipants([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _directChatParticipantService.GetAllDirectChatParticipant(pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy chi tiết theo ID.
        /// </summary>
        /// <remarks>
        /// - Nhận `id` của cuộc trò chuyện và trả về thông tin chi tiết.  
        /// - Nếu cuộc trò chuyện không tồn tại, trả về lỗi `404 Not Found`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID cần lấy.</param>
        /// <returns>
        /// - `200 OK`: Trả về thông tin thành công.  
        /// - `404 Not Found`: Không tìm thấy.
        /// </returns>
        [HttpGet(ApiEndPointConstant.DirectChatParticipant.GetDirectChatParticipantById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetDirectChatParticipantById([FromRoute] Guid id)
        {
            var response = await _directChatParticipantService.GetDirectChatParticipantById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API cập nhật theo ID.
        /// </summary>
        /// <remarks>
        /// - Nhận `id` của cuộc trò chuyện và dữ liệu cần cập nhật (`UpdateDirectChatParticipant`).  
        /// - Nếu không tồn tại, trả về lỗi `404 Not Found`.  
        /// - Nếu dữ liệu không hợp lệ, trả về lỗi `400 Bad Request`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID cần cập nhật.</param>
        /// <param name="request">Dữ liệu cập nhật cuộc trò chuyện.</param>
        /// <returns>
        /// - `200 OK`: Cập nhậtn thành công.  
        /// - `404 Not Found`: Không tìm thấy.  
        /// - `400 Bad Request`: Dữ liệu không hợp lệ.
        /// </returns>
        [HttpPut(ApiEndPointConstant.DirectChatParticipant.UpdateDirectChatParticipant)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateDirectChatParticipant([FromRoute] Guid id, [FromBody] UpdateDirectChatParticipant request)
        {
            var response = await _directChatParticipantService.UpdateDirectChatParticipant(id, request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API xóa theo ID.
        /// </summary>
        /// <remarks>
        /// - Nhận `id` cần xóa.  
        /// - Nếu không tồn tại, trả về lỗi `404 Not Found`.  
        /// - Nếu không thể xóa, trả về lỗi `400 Bad Request`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của cuộc trò chuyện cần xóa.</param>
        /// <returns>
        /// - `200 OK`: Xóa thành công.  
        /// - `404 Not Found`: Không tìm thấy.  
        /// - `400 Bad Request`: Không thể xóa.
        /// </returns>
        [HttpDelete(ApiEndPointConstant.DirectChatParticipant.RemoveDirectChatParticipant)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RemoveDirectChatParticipant([FromRoute] Guid id)
        {
            var response = await _directChatParticipantService.RemoveDirectChatParticipant(id);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}
