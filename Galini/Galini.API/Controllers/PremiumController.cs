using Galini.API.Constants;
using Galini.Models.Payload.Request.Premium;
using Galini.Models.Payload.Request.UserInfo;
using Galini.Models.Payload.Response;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class PremiumController : BaseController<PremiumController>
    {
        private readonly IPremiumService _premiumService;
        public PremiumController(ILogger<PremiumController> logger, IPremiumService premiumService) : base(logger)
        {
            _premiumService = premiumService;
        }

        /// <summary>
        /// API tạo gói Premium.
        /// </summary>
        /// <remarks>
        /// - Nhận thông tin gói Premium (`CreatePremiumRequest`).  
        /// - Trả về `BaseResponse` chứa thông tin gói Premium vừa tạo.  
        /// - Nếu dữ liệu đầu vào không hợp lệ, trả về lỗi `400 Bad Request`.  
        /// </remarks>
        /// <param name="request">Thông tin gói Premium cần tạo.</param>
        /// <returns>
        /// - `200 OK`: Tạo gói Premium thành công.  
        /// - `400 Bad Request`: Dữ liệu không hợp lệ.  
        /// </returns>
        [HttpPost(ApiEndPointConstant.Premium.CreatePremium)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreatePremium([FromBody] CreatePremiumRequest request)
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

            var response = await _premiumService.CreatePremium(request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách gói Premium có phân trang và lọc.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách premium có hỗ trợ phân trang.  
        /// - Nếu không truyền `page` hoặc `size`, giá trị mặc định sẽ được sử dụng (`page = 1`, `size = 10`).  
        /// - Có thể lọc theo số lượng bạn bè, thời gian, match, khoảng giá
        /// - Hỗ trợ sắp xếp theo số bạn bè, số match, giá, thời gian.  
        /// - Trả về `BaseResponse` chứa danh sách gói Premium.  
        /// </remarks>
        /// <param name="page">Trang hiện tại (mặc định = 1).</param>
        /// <param name="size">Số lượng gói trên mỗi trang (mặc định = 10).</param>
        /// <param name="friend">Lọc theo số lượng bạn bè.</param>
        /// <param name="timelimit">Lọc theo thời gian hiệu lực.</param>
        /// <param name="match">Lọc theo số match.</param>
        /// <param name="minPrice">Lọc theo giá tối thiểu.</param>
        /// <param name="maxPrice">Lọc theo giá tối đa.</param>
        /// <param name="sortByFriend">Sắp xếp theo số lượng bạn bè.</param>
        /// <param name="sortByMatch">Sắp xếp theo số match.</param>
        /// <param name="sortByPrice">Sắp xếp theo giá.</param>
        /// <param name="sortByTimelimit">Sắp xếp theo thời gian hiệu lực.</param>
        /// <returns>
        /// - `200 OK`: Lấy danh sách gói Premium thành công.  
        /// </returns>
        [HttpGet(ApiEndPointConstant.Premium.GetAllPremium)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllPremium([FromQuery] int? page, [FromQuery] int? size,
                                                                            [FromQuery] int? friend,
                                                                            [FromQuery] bool? timelimit,
                                                                            [FromQuery] int? match,
                                                                            [FromQuery] double? minPrice,
                                                                            [FromQuery] double? maxPrice,
                                                                            [FromQuery] bool? sortByFriend,
                                                                            [FromQuery] bool? sortByMatch,
                                                                            [FromQuery] bool? sortByPrice,
                                                                            [FromQuery] bool? sortByTimelimit)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _premiumService.GetAllPremium(pageNumber, pageSize, friend, timelimit, match, minPrice, maxPrice, sortByFriend, sortByMatch, sortByPrice, sortByTimelimit);

            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy thông tin gói Premium theo ID.
        /// </summary>
        /// <remarks>
        /// - Nhận `id` của gói Premium cần lấy thông tin.  
        /// - Trả về `BaseResponse` chứa thông tin chi tiết của gói Premium.  
        /// - Nếu không tìm thấy gói Premium, trả về lỗi `404 Not Found`.  
        /// </remarks>
        /// <param name="id">ID của gói Premium.</param>
        /// <returns>
        /// - `200 OK`: Lấy thông tin thành công.  
        /// - `404 Not Found`: Không tìm thấy gói Premium.  
        /// </returns>
        [HttpGet(ApiEndPointConstant.Premium.GetPremiumById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetPremiumById([FromRoute] Guid id)
        {
            var response = await _premiumService.GetPremiumById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API cập nhật thông tin gói Premium.
        /// </summary>
        /// <remarks>
        /// - Nhận `id` của gói Premium và dữ liệu cần cập nhật (`UpdatePremiumRequest`).  
        /// - Trả về `BaseResponse` chứa thông tin gói Premium đã cập nhật.  
        /// - Nếu không tìm thấy gói Premium, trả về lỗi `404 Not Found`.  
        /// </remarks>
        /// <param name="id">ID của gói Premium cần cập nhật.</param>
        /// <param name="request">Thông tin cập nhật gói Premium.</param>
        /// <returns>
        /// - `200 OK`: Cập nhật thành công.  
        /// - `404 Not Found`: Không tìm thấy gói Premium hoặc dữ liệu không hợp lệ.  
        /// </returns>
        [HttpPatch(ApiEndPointConstant.Premium.UpdatePremium)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateUserInfo([FromRoute] Guid id, [FromBody] UpdatePremiumRequest request)
        {

            var response = await _premiumService.UpdatePremium(id, request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API xóa gói Premium.
        /// </summary>
        /// <remarks>
        /// - Nhận `id` của gói Premium cần xóa.  
        /// - Nếu không tìm thấy gói Premium, trả về lỗi `404 Not Found`.  
        /// </remarks>
        /// <param name="id">ID của gói Premium cần xóa.</param>
        /// <returns>
        /// - `200 OK`: Xóa thành công.  
        /// - `404 Not Found`: Không tìm thấy gói Premium.  
        /// </returns>
        [HttpDelete(ApiEndPointConstant.Premium.RemovePremium)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RemovePremium([FromRoute] Guid id)
        {
            var response = await _premiumService.RemovePremium(id);

            return StatusCode(int.Parse(response.status), response);
        }
    }
}
