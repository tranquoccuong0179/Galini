
using Galini.API.Constants;
using Galini.Models.Payload.Request.Authenticaion;
using Galini.Models.Payload.Response;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class DashboardController : BaseController<DashboardController>
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(ILogger<DashboardController> logger, IDashboardService dashboardService) : base(logger)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// API lấy thông tin tổng quan của bảng điều khiển (Dashboard).
        /// </summary>
        /// <remarks>
        /// - Lấy dữ liệu tổng quan để hiển thị trên bảng điều khiển, bao gồm các thông số như tổng số người nghe, người dùng, bài viết và giao dịch.  
        /// - Dữ liệu biểu đồ (`ChartData`) cũng được trả về để hỗ trợ hiển thị biểu đồ.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.  
        /// </remarks>
        /// <returns>
        /// - `200 OK`: Lấy thông tin Dashboard thành công.  
        ///   Response sẽ chứa các thuộc tính sau:  
        ///   - `TotalListeners` (int): Tổng số người nghe.  
        ///   - `TotalUsers` (int): Tổng số người dùng.  
        ///   - `TotalBlogs` (int): Tổng số bài viết.  
        ///   - `TotalTransaction` (decimal): Tổng giá trị giao dịch.  
        ///   - `Chart` (`ChartData`): Dữ liệu biểu đồ, bao gồm:  
        ///     - `Labels` (List&lt;string&gt;): Danh sách nhãn của biểu đồ (ví dụ: danh sách các ngày, tháng,...).  
        ///     - `Values` (List&lt;int&gt;): Danh sách giá trị tương ứng với các nhãn (ví dụ: số lượng người dùng theo ngày, tháng,...).  
        /// - `500 Internal Server Error`: Lỗi xảy ra trong quá trình xử lý (nếu có).  
        /// </returns>
        [HttpGet(ApiEndPointConstant.Dashboard.GetDashboard)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetDashboard()
        {
            var response = await _dashboardService.GetDashboard();
            return StatusCode(int.Parse(response.status), response);
        }
    }
}
