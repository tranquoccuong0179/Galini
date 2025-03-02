using Galini.API.Constants;
using Galini.Models.Enum;
using Galini.Models.Payload.Request.CallHistory;
using Galini.Models.Payload.Request.FriendShip;
using Galini.Models.Payload.Request.Review;
using Galini.Models.Payload.Response;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class ReviewController : BaseController<ReviewController>
    {
        private readonly IReviewService _reviewService;
        public ReviewController(ILogger<ReviewController> logger, IReviewService reviewService) : base(logger)
        {
            _reviewService = reviewService;
        }

        /// <summary>
        /// API tạo đánh giá mới.
        /// </summary>
        /// <remarks>
        /// - Tạo đánh giá dựa trên `request` và `bookingId`.  
        /// - Kiểm tra booking và người nghe có tồn tại không, nếu không trả `404`.  
        /// - Nếu dữ liệu `request` không hợp lệ, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="request">Dữ liệu đánh giá cần tạo.</param>
        /// <param name="bookingId">ID của booking liên quan.</param>
        /// <returns>
        /// - `200 OK`: Tạo đánh giá thành công.  
        /// - `400 Bad Request`: Dữ liệu không hợp lệ hoặc lưu thất bại.  
        /// - `404 Not Found`: Không tìm thấy booking hoặc người nghe.
        /// </returns>
        [HttpPost(ApiEndPointConstant.Review.CreateReview)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequest request, [FromQuery] Guid bookingId)
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

            var response = await _reviewService.CreateReview(request, bookingId);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách đánh giá với phân trang.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách đánh giá có phân trang, mặc định `page = 1`, `size = 10`.  
        /// - Lọc theo số sao (`star`) và booking (`id`) nếu có, sắp xếp theo sao (`sortByStar`) hoặc thời gian tạo.  
        /// - Nếu `sortByStar = true` thì tăng dần, `false` thì giảm dần, không truyền thì theo thời gian tạo.  
        /// - Nếu `page` hoặc `size` nhỏ hơn 1, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="page">Số trang (mặc định 1).</param>
        /// <param name="size">Số lượng mỗi trang (mặc định 10).</param>
        /// <param name="star">Lọc theo số sao tối thiểu.</param>
        /// <param name="sortByStar">Sắp xếp theo sao (true: tăng, false: giảm).</param>
        /// <param name="id">ID booking để lọc.</param>
        /// <returns>
        /// - `200 OK`: Lấy danh sách đánh giá thành công.  
        /// - `400 Bad Request`: Page hoặc size không hợp lệ.
        /// </returns>
        [HttpGet(ApiEndPointConstant.Review.GetAllReview)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllReview([FromQuery] int? page, [FromQuery] int? size, [FromQuery] int? star,
                                                                                                    [FromQuery] bool? sortByStar,
                                                                                                    [FromQuery] Guid? id)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _reviewService.GetAllReview(pageNumber, pageSize, star, sortByStar, id);

            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách đánh giá theo ID tham vấn viên với phân trang.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách đánh giá của tham vấn viên dựa trên `id`, có phân trang (mặc định `page = 1`, `size = 10`).  
        /// - Lọc theo số sao (`star`) nếu có, sắp xếp theo sao (`sortByStar`) hoặc thời gian tạo.  
        /// - Nếu `sortByStar = true` thì tăng dần, `false` thì giảm dần, không truyền thì theo thời gian tạo.  
        /// - Nếu `page` hoặc `size` nhỏ hơn 1, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của tham vấn viên.</param>
        /// <param name="star">Lọc theo số sao tối thiểu.</param>
        /// <param name="sortByStar">Sắp xếp theo sao (true: tăng, false: giảm).</param>
        /// <param name="page">Số trang (mặc định 1).</param>
        /// <param name="size">Số lượng mỗi trang (mặc định 10).</param>
        /// <returns>
        /// - `200 OK`: Lấy danh sách đánh giá thành công.  
        /// - `400 Bad Request`: Page hoặc size không hợp lệ.
        /// </returns>
        [HttpGet(ApiEndPointConstant.Review.GetAllReviewByListenerId)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllReviewByListenerId([FromRoute] Guid id, [FromQuery] int? star, [FromQuery] bool? sortByStar, [FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _reviewService.GetAllReviewByListenerId(pageNumber, pageSize, star, sortByStar, id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy thông tin đánh giá theo ID.
        /// </summary>
        /// <remarks>
        /// - Trả về chi tiết đánh giá dựa trên `id`.  
        /// - Chỉ lấy đánh giá còn hoạt động (`IsActive = true`).  
        /// - Nếu không tìm thấy, trả lỗi `404`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của đánh giá.</param>
        /// <returns>
        /// - `200 OK`: Lấy đánh giá thành công.  
        /// - `404 Not Found`: Không tìm thấy đánh giá.
        /// </returns>
        [HttpGet(ApiEndPointConstant.Review.GetReviewById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetReviewById([FromRoute] Guid id)
        {
            var response = await _reviewService.GetReviewById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API cập nhật thông tin đánh giá theo ID.
        /// </summary>
        /// <remarks>
        /// - Cập nhật đánh giá dựa trên `id` và `request`.  
        /// - Nếu `id` không tồn tại hoặc không hoạt động, trả lỗi `404`.  
        /// - Nếu dữ liệu `request` không hợp lệ hoặc cập nhật thất bại, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của đánh giá cần cập nhật.</param>
        /// <param name="request">Thông tin cập nhật cho đánh giá.</param>
        /// <returns>
        /// - `200 OK`: Cập nhật đánh giá thành công.  
        /// - `400 Bad Request`: Dữ liệu không hợp lệ hoặc cập nhật thất bại.  
        /// - `404 Not Found`: Không tìm thấy đánh giá.
        /// </returns>
        [HttpPatch(ApiEndPointConstant.Review.UpdateReview)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateReview([FromRoute] Guid id, [FromBody] UpdateReviewRequest request)
        {

            var response = await _reviewService.UpdateReview(id, request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API xoá đánh giá theo ID.
        /// </summary>
        /// <remarks>
        /// - Xoá đánh giá dựa trên `id` (chuyển `IsActive = false`).  
        /// - Nếu không tìm thấy đánh giá, trả lỗi `404`.  
        /// - Nếu không xoá được, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của đánh giá cần xoá.</param>
        /// <returns>
        /// - `200 OK`: Xoá đánh giá thành công.  
        /// - `400 Bad Request`: Không thể xoá đánh giá.  
        /// - `404 Not Found`: Không tìm thấy đánh giá.
        /// </returns>
        [HttpDelete(ApiEndPointConstant.Review.RemoveReview)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RemoveReview([FromRoute] Guid id)
        {
            var response = await _reviewService.RemoveReview(id);

            return StatusCode(int.Parse(response.status), response);
        }
    }    
}
