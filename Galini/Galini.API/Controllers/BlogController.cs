
using AngleSharp.Io;
using Galini.API.Constants;
using Galini.Models.Payload.Request.Blog;
using Galini.Models.Payload.Request.Question;
using Galini.Models.Payload.Response;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class BlogController : BaseController<BlogController>
    {
        private readonly IBlogService _blogService;
        public BlogController(ILogger<BlogController> logger, IBlogService blogService) : base(logger)
        {
            _blogService = blogService;
        }

        /// <summary>
        /// API tạo bài viết mới.
        /// </summary>
        /// <remarks>
        /// - Nhận dữ liệu từ client dưới dạng `CreateBlogRequest`.  
        /// - `Content` sử dụng react quill
        /// - Kiểm tra tính hợp lệ của dữ liệu trước khi tạo bài viết.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="request">Dữ liệu bài viết cần tạo.</param>
        /// <returns>
        /// - `200 OK`: Tạo bài viết thành công.  
        /// - `400 Bad Request`: Yêu cầu không hợp lệ.
        /// </returns>
        [HttpPost(ApiEndPointConstant.Blog.CreateBlog)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateBlog([FromBody] CreateBlogRequest request)
        {
            var response = await _blogService.CreateBlog(request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách bài viết với phân trang.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách bài viết có hỗ trợ phân trang.  
        /// - Nếu không truyền `page` hoặc `size`, giá trị mặc định sẽ được sử dụng (`page = 1`, `size = 10`).  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="page">Số trang hiện tại (mặc định là 1).</param>
        /// <param name="size">Số lượng bài viết trên mỗi trang (mặc định là 10).</param>
        /// <returns>
        /// - `200 OK`: Trả về danh sách bài viết thành công.
        /// </returns>
        [HttpGet(ApiEndPointConstant.Blog.GetAllBlogs)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllBlogs([FromQuery] int? page,
                                                     [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _blogService.GetAllBlogs(pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy chi tiết một bài viết theo ID.
        /// </summary>
        /// <remarks>
        /// - Nhận `id` của bài viết và trả về thông tin chi tiết.  
        /// - Nếu bài viết không tồn tại, trả về lỗi `404 Not Found`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của bài viết cần lấy.</param>
        /// <returns>
        /// - `200 OK`: Trả về thông tin bài viết thành công.  
        /// - `404 Not Found`: Không tìm thấy bài viết.
        /// </returns>
        [HttpGet(ApiEndPointConstant.Blog.GetBlogById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetBlogById([FromRoute] Guid id)
        {
            var response = await _blogService.GetBlogById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API xóa một bài viết theo ID.
        /// </summary>
        /// <remarks>
        /// - Nhận `id` của bài viết cần xóa.  
        /// - Nếu bài viết không tồn tại, trả về lỗi `404 Not Found`.  
        /// - Nếu không thể xóa, trả về lỗi `400 Bad Request`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của bài viết cần xóa.</param>
        /// <returns>
        /// - `200 OK`: Xóa bài viết thành công.  
        /// - `404 Not Found`: Không tìm thấy bài viết.  
        /// - `400 Bad Request`: Không thể xóa bài viết.
        /// </returns>
        [HttpDelete(ApiEndPointConstant.Blog.RemoveBlog)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RemoveBlog([FromRoute] Guid id)
        {
            var response = await _blogService.RemoveBlog(id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API cập nhật thông tin bài viết.
        /// </summary>
        /// <remarks>
        /// - Nhận `id` của bài viết và dữ liệu cần cập nhật (`UpdateBlogRequest`).  
        /// - Nếu bài viết không tồn tại, trả về lỗi `404 Not Found`.  
        /// - Nếu dữ liệu không hợp lệ, trả về lỗi `400 Bad Request`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của bài viết cần cập nhật.</param>
        /// <param name="request">Dữ liệu cập nhật bài viết.</param>
        /// <returns>
        /// - `200 OK`: Cập nhật bài viết thành công.  
        /// - `404 Not Found`: Không tìm thấy bài viết.  
        /// - `400 Bad Request`: Dữ liệu không hợp lệ.
        /// </returns>
        [HttpPut(ApiEndPointConstant.Blog.UpdateBlog)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateQuestion([FromRoute] Guid id, [FromBody] UpdateBlogRequest request)
        {
            var response = await _blogService.UpdateBlog(id, request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API like một bài viết theo ID.
        /// </summary>
        /// <remarks>
        /// - Nhận `id` của bài viết và cập nhật lượt like.  
        /// - Nếu bài viết không tồn tại, trả về lỗi `404 Not Found`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của bài viết cần lấy.</param>
        /// <returns>
        /// - `200 OK`: Trả về thông tin bài viết thành công.  
        /// - `400 Bad Request`: Like bài viết thất bại.  
        /// - `404 Not Found`: Không tìm thấy bài viết.
        /// </returns>
        [HttpGet(ApiEndPointConstant.Blog.LikeBlog)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> LikeBlog([FromRoute] Guid id)
        {
            var response = await _blogService.LikeBlog(id);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpPost(ApiEndPointConstant.Blog.UploadImg)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UploadImg(IFormFile formFile)
        {

            var response = await _blogService.UpImageForDescription(formFile);

            return StatusCode(int.Parse(response.status), response);

        }

        [HttpGet(ApiEndPointConstant.Blog.GetAllBlogsByUser)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllBlogsByUser()
        {

            var response = await _blogService.GetAllBlogByUser();

            return StatusCode(int.Parse(response.status), response);

        }
    }
}
