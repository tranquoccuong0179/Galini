
using Galini.API.Constants;
using Galini.Models.Enum;
using Galini.Models.Payload.Response;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class TransactionController : BaseController<TransactionController>
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ILogger<TransactionController> logger, ITransactionService transactionService) : base(logger)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// API lấy danh sách giao dịch.
        /// </summary>
        /// <remarks>
        /// - Hỗ trợ phân trang với tham số `page` và `size`.  
        /// - Chỉ người dùng có quyền "Customer" mới được truy cập và đúng tài khoản. 
        /// - Hỗ trợ lọc giao dịch theo thời gian (`daysAgo`, `weeksAgo`, `monthsAgo`).  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="page">Số trang hiện tại (mặc định là `1`).</param>
        /// <param name="size">Số lượng giao dịch trên mỗi trang (mặc định là `10`).</param>
        /// <param name="daysAgo">Lọc giao dịch trong `x` ngày trước.</param>
        /// <param name="weeksAgo">Lọc giao dịch trong `x` tuần trước.</param>
        /// <param name="monthsAgo">Lọc giao dịch trong `x` tháng trước.</param>
        /// <returns>
        /// - `200 OK`: Lấy danh sách giao dịch thành công.
        /// </returns>  
        [HttpGet(ApiEndPointConstant.Transaction.GetTransactions)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetTransactions([FromQuery] int? page, 
                                                         [FromQuery] int? size, 
                                                         [FromQuery] int? daysAgo, 
                                                         [FromQuery] int? weeksAgo, 
                                                         [FromQuery] int? monthsAgo)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _transactionService.GetTransactions(pageNumber, pageSize, daysAgo, weeksAgo, monthsAgo);

            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách giao dịch dành cho Admin.
        /// </summary>
        /// <remarks>
        /// - Hỗ trợ phân trang với tham số `page` và `size`.  
        /// - Chỉ người dùng có quyền "Admin"
        /// - Hỗ trợ tìm kiếm theo tên, email, số điện thoại.  
        /// - Hỗ trợ lọc theo trạng thái, loại giao dịch, khoảng thời gian.  
        /// - Có thể sắp xếp theo giá trị giao dịch (`sortByPrice`).  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="page">Số trang hiện tại (mặc định là `1`).</param>
        /// <param name="size">Số lượng giao dịch trên mỗi trang (mặc định là `10`).</param>
        /// <param name="name">Lọc giao dịch theo tên người dùng.</param>
        /// <param name="email">Lọc giao dịch theo email.</param>
        /// <param name="phone">Lọc giao dịch theo số điện thoại.</param>
        /// <param name="status">Lọc giao dịch theo trạng thái.</param>
        /// <param name="type">Lọc giao dịch theo loại giao dịch.</param>
        /// <param name="sortByPrice">Sắp xếp theo giá trị giao dịch (`true`: tăng dần, `false`: giảm dần).</param>
        /// <param name="daysAgo">Lọc giao dịch trong `x` ngày trước.</param>
        /// <param name="weeksAgo">Lọc giao dịch trong `x` tuần trước.</param>
        /// <param name="monthsAgo">Lọc giao dịch trong `x` tháng trước.</param>
        /// <returns>
        /// - `200 OK`: Lấy danh sách giao dịch thành công.
        /// </returns>
        [HttpGet(ApiEndPointConstant.Transaction.GetTransactionsForAdmin)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetTransactionsForAdmin([FromQuery] int? page, 
                                                                 [FromQuery] int? size,
                                                                 [FromQuery] string? name,
                                                                 [FromQuery] string? email,
                                                                 [FromQuery] string? phone,
                                                                 [FromQuery] TransactionStatusEnum? status,
                                                                 [FromQuery] TransactionTypeEnum? type,
                                                                 [FromQuery] bool? sortByPrice,
                                                                 [FromQuery] int? daysAgo,
                                                                 [FromQuery] int? weeksAgo,
                                                                 [FromQuery] int? monthsAgo)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _transactionService.GetAllTransaction(pageNumber, pageSize, name, email, phone, status, type, sortByPrice, daysAgo, weeksAgo, monthsAgo);

            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy thông tin chi tiết của một giao dịch.
        /// </summary>
        /// <remarks>
        /// - Nhận `id` của giao dịch và trả về thông tin chi tiết.  
        /// - Nếu giao dịch không tồn tại, trả về lỗi `404 Not Found`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của giao dịch cần lấy.</param>
        /// <returns>
        /// - `200 OK`: Trả về thông tin giao dịch thành công.  
        /// - `404 Not Found`: Không tìm thấy giao dịch.
        /// </returns>
        [HttpGet(ApiEndPointConstant.Transaction.GetTransactionById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetTransactionById([FromRoute] Guid id)
        {
            var response = await _transactionService.GetTransaction(id);

            return StatusCode(int.Parse(response.status), response);
        }
    }
}
