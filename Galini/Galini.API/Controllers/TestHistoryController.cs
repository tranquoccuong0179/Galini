using Galini.API.Constants;
using Galini.Models.Payload.Request.TestHistory;
using Galini.Models.Payload.Response;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class TestHistoryController : BaseController<TestHistoryController>
    {
        private readonly ITestHistoryService _testHistoryService;
        public TestHistoryController(ILogger<TestHistoryController> logger, ITestHistoryService testHistoryService) : base(logger)
        {
            _testHistoryService = testHistoryService;
        }

        /// <summary>
        /// - API tạo lịch sử kiểm tra mới, mỗi khi người dùng hoàn thành bài kiểm tra.
        /// - Chỉ lưu điểm và status của người dùng, việc tính điểm và từ điểm đưa ra status được thực hiện ở FE
        /// </summary>
        /// <remarks>
        /// - Tạo lịch sử kiểm tra dựa trên `request`.  
        /// - Kiểm tra tài khoản người dùng có tồn tại không, nếu không trả `404`.  
        /// - Nếu dữ liệu `request` không hợp lệ, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="request">Dữ liệu lịch sử kiểm tra cần tạo.</param>
        /// <returns>
        /// - `200 OK`: Tạo lịch sử kiểm tra thành công.  
        /// - `400 Bad Request`: Dữ liệu không hợp lệ hoặc lưu thất bại.  
        /// - `404 Not Found`: Không tìm thấy tài khoản người dùng.
        /// </returns>
        [HttpPost(ApiEndPointConstant.TestHistory.CreateTestHistory)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateTestHistory([FromBody] CreateTestHistoryRequest request)
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

            var response = await _testHistoryService.CreateTestHistory(request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách lịch sử kiểm tra với phân trang.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách lịch sử kiểm tra, mặc định `page = 1`, `size = 10`.  
        /// - Lọc theo điểm (`grade`) và trạng thái (`status`) nếu có, sắp xếp theo điểm (`sortByGrade`) hoặc thời gian tạo.  
        /// - Nếu `sortByGrade = true` thì tăng dần, `false` thì giảm dần, không truyền thì theo thời gian tạo.  
        /// - Nếu `page` hoặc `size` nhỏ hơn 1, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="page">Số trang (mặc định 1).</param>
        /// <param name="size">Số lượng mỗi trang (mặc định 10).</param>
        /// <param name="grade">Lọc theo điểm tối thiểu.</param>
        /// <param name="status">Lọc theo trạng thái.</param>
        /// <param name="sortByGrade">Sắp xếp theo điểm (true: tăng, false: giảm).</param>
        /// <returns>
        /// - `200 OK`: Lấy danh sách lịch sử kiểm tra thành công.  
        /// - `400 Bad Request`: Page hoặc size không hợp lệ.
        /// </returns>
        [HttpGet(ApiEndPointConstant.TestHistory.GetAllTestHistory)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllTestHistory([FromQuery] int? page, [FromQuery] int? size, [FromQuery] int? grade, [FromQuery] string? status, [FromQuery] bool? sortByGrade)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _testHistoryService.GetAllTestHistory(pageNumber, pageSize, grade, status, sortByGrade);

            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy thông tin lịch sử kiểm tra theo ID.
        /// </summary>
        /// <remarks>
        /// - Trả về chi tiết lịch sử kiểm tra dựa trên `id`.  
        /// - Chỉ lấy lịch sử còn hoạt động (`IsActive = true`).  
        /// - Nếu không tìm thấy, trả lỗi `404`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của lịch sử kiểm tra.</param>
        /// <returns>
        /// - `200 OK`: Lấy lịch sử kiểm tra thành công.  
        /// - `404 Not Found`: Không tìm thấy lịch sử kiểm tra.
        /// </returns>
        [HttpGet(ApiEndPointConstant.TestHistory.GetTestHistoryById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetTestHistoryById([FromRoute] Guid id)
        {
            var response = await _testHistoryService.GetTestHistoryById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách lịch sử kiểm tra theo ID tài khoản với phân trang.
        /// </summary>
        /// <remarks>
        /// - Trả về lịch sử kiểm tra của tài khoản hiện tại, mặc định `page = 1`, `size = 10`.  
        /// - Lọc theo điểm (`grade`) và trạng thái (`status`) nếu có, sắp xếp theo điểm (`sortByGrade`) hoặc thời gian tạo.  
        /// - Nếu `sortByGrade = true` thì tăng dần, `false` thì giảm dần, không truyền thì theo thời gian tạo.  
        /// - Nếu tài khoản không tồn tại, trả `404`. Nếu `page` hoặc `size` nhỏ hơn 1, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="page">Số trang (mặc định 1).</param>
        /// <param name="size">Số lượng mỗi trang (mặc định 10).</param>
        /// <param name="grade">Lọc theo điểm tối thiểu.</param>
        /// <param name="status">Lọc theo trạng thái.</param>
        /// <param name="sortByGrade">Sắp xếp theo điểm (true: tăng, false: giảm).</param>
        /// <returns>
        /// - `200 OK`: Lấy danh sách lịch sử kiểm tra thành công.  
        /// - `400 Bad Request`: Page hoặc size không hợp lệ.  
        /// - `404 Not Found`: Không tìm thấy tài khoản.
        /// </returns>
        [HttpGet(ApiEndPointConstant.TestHistory.GetTestHistoryByAccountId)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetTestHistoryByAccountId([FromQuery] int? page, [FromQuery] int? size, [FromQuery] int? grade, [FromQuery] string? status, [FromQuery] bool? sortByGrade)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _testHistoryService.GetTestHistoryByAccountId(pageNumber, pageSize, grade, status, sortByGrade);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API cập nhật thông tin lịch sử kiểm tra theo ID.
        /// </summary>
        /// <remarks>
        /// - Cập nhật lịch sử kiểm tra dựa trên `id` và `request`.  
        /// - Nếu `id` không tồn tại hoặc không hoạt động, trả lỗi `404`.  
        /// - Nếu dữ liệu `request` không hợp lệ hoặc cập nhật thất bại, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của lịch sử kiểm tra cần cập nhật.</param>
        /// <param name="request">Thông tin cập nhật cho lịch sử kiểm tra.</param>
        /// <returns>
        /// - `200 OK`: Cập nhật lịch sử kiểm tra thành công.  
        /// - `400 Bad Request`: Dữ liệu không hợp lệ hoặc cập nhật thất bại.  
        /// - `404 Not Found`: Không tìm thấy lịch sử kiểm tra.
        /// </returns>
        [HttpPatch(ApiEndPointConstant.TestHistory.UpdateTestHistory)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateTestHistory([FromRoute] Guid id, [FromBody] UpdateTestHistoryRequest request)
        {

            var response = await _testHistoryService.UpdateTestHistory(id, request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API xoá lịch sử kiểm tra theo ID.
        /// </summary>
        /// <remarks>
        /// - Xoá lịch sử kiểm tra dựa trên `id` (chuyển `IsActive = false`).  
        /// - Nếu không tìm thấy lịch sử kiểm tra, trả lỗi `404`.  
        /// - Nếu không xoá được, trả lỗi `400`.  
        /// - Kết quả bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của lịch sử kiểm tra cần xoá.</param>
        /// <returns>
        /// - `200 OK`: Xoá lịch sử kiểm tra thành công.  
        /// - `400 Bad Request`: Không thể xoá lịch sử kiểm tra.  
        /// - `404 Not Found`: Không tìm thấy lịch sử kiểm tra.
        /// </returns>
        [HttpDelete(ApiEndPointConstant.TestHistory.RemoveTestHistory)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RemoveTestHistory([FromRoute] Guid id)
        {
            var response = await _testHistoryService.RemoveTestHistory(id);

            return StatusCode(int.Parse(response.status), response);
        }
    }
}
