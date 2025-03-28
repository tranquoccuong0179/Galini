using Galini.API.Constants;
using Galini.Models.Enum;
using Galini.Models.Payload.Request.CallHistory;
using Galini.Models.Payload.Request.FriendShip;
using Galini.Models.Payload.Request.UserInfo;
using Galini.Models.Payload.Response;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class FriendShipController : BaseController<FriendShipController>
    {
        private readonly IFriendShipService _friendShipService;
        public FriendShipController(ILogger<FriendShipController> logger, IFriendShipService friendShipService) : base(logger)
        {
            _friendShipService = friendShipService;
        }

        /// <summary>
        /// API tạo mới mối quan hệ bạn bè, mỗi khi người dùng gửi lời mời kết bạn thì tạo.
        /// </summary>
        /// <remarks>
        /// - Tạo mối quan hệ bạn bè giữa `userId` và `friendId`.  
        /// - Nếu dữ liệu đầu vào không hợp lệ, trả về lỗi `400 Bad Request`.  
        /// - Nếu tạo thành công, trả về thông tin mối quan hệ bạn bè vừa tạo.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.  
        /// </remarks>
        /// <param name="userId">ID của người dùng.</param>
        /// <param name="friendId">ID của người bạn.</param>
        /// <returns>
        /// - `200 OK`: Tạo mối quan hệ bạn bè thành công.  
        /// - `400 Bad Request`: Dữ liệu không hợp lệ hoặc không thể tạo mối quan hệ bạn bè.  
        /// - `404 Not Found`: Không tìm thấy user hoặc friend.  
        /// </returns>
        [HttpPost(ApiEndPointConstant.FriendShip.CreateFriendShip)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateFriendShip([FromQuery] Guid userId, [FromQuery] Guid friendId)
        {
            var response = await _friendShipService.CreateFriendShip(userId, friendId);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách mối quan hệ bạn bè với phân trang, hỗ trợ lọc và sắp xếp.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách mối quan hệ bạn bè có hỗ trợ phân trang.  
        /// - Nếu không truyền `page` hoặc `size`, giá trị mặc định sẽ được sử dụng (`page = 1`, `size = 10`).  
        /// - Có thể lọc theo `status` của mối quan hệ bạn bè.  
        /// - Khi `sortByStatus` là `true`, danh sách sẽ được sắp xếp theo trạng thái tăng dần, `false` là giảm dần.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.  
        /// </remarks>
        /// <param name="page">Số trang hiện tại (mặc định là 1).</param>
        /// <param name="size">Số lượng bản ghi trên mỗi trang (mặc định là 10).</param>
        /// <param name="status">Lọc danh sách theo trạng thái mối quan hệ bạn bè.</param>
        /// <param name="sortByStatus">Sắp xếp theo trạng thái (`true`: tăng dần, `false`: giảm dần).</param>
        /// <returns>
        /// - `200 OK`: Trả về danh sách mối quan hệ bạn bè thành công.  
        /// - `400 Bad Request`: Yêu cầu không hợp lệ.  
        /// </returns>
        [HttpGet(ApiEndPointConstant.FriendShip.GetAllFriendShip)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllFriendShip([FromQuery] int? page, [FromQuery] int? size, [FromQuery] string? status, [FromQuery] bool? sortByStatus)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _friendShipService.GetAllFriendShip(pageNumber, pageSize, status, sortByStatus);

            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách bạn bè của người dùng hiện tại.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách bạn bè của người dùng hiện tại với hỗ trợ phân trang.  
        /// - Người dùng được xác định dựa trên thông tin trong `HttpContext`.  
        /// - Nếu tài khoản không tồn tại hoặc không hợp lệ, trả về lỗi `404 Not Found`.  
        /// - Nếu người dùng không có bạn bè nào, trả về thông báo `"Không tìm thấy bạn bè nào."` với `200 OK`.  
        /// - Nếu không truyền `page` hoặc `size`, giá trị mặc định sẽ được sử dụng (`page = 1`, `size = 10`).  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="page">Số trang hiện tại (mặc định là 1).</param>
        /// <param name="size">Số lượng bạn bè trên mỗi trang (mặc định là 10).</param>
        /// <returns>
        /// - `200 OK`: Trả về danh sách bạn bè thành công.  
        /// - `400 Bad Request`: Tham số `page` hoặc `size` không hợp lệ.  
        /// - `404 Not Found`: Không tìm thấy tài khoản người dùng.
        /// </returns>
        [HttpGet(ApiEndPointConstant.FriendShip.GetFriendList)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetFriendList([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _friendShipService.GetFriendList(pageNumber, pageSize);

            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy thông tin mối quan hệ bạn bè theo ID.
        /// </summary>
        /// <remarks>
        /// - Trả về thông tin chi tiết của một mối quan hệ bạn bè dựa trên `id`.  
        /// - Nếu không tìm thấy mối quan hệ, trả về lỗi `404 Not Found`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của mối quan hệ bạn bè cần lấy thông tin.</param>
        /// <returns>
        /// - `200 OK`: Trả về thông tin mối quan hệ bạn bè thành công.  
        /// - `404 Not Found`: Không tìm thấy mối quan hệ bạn bè.
        /// </returns>
        [HttpGet(ApiEndPointConstant.FriendShip.GetFriendShipById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetFriendShipById([FromRoute] Guid id)
        {
            var response = await _friendShipService.GetFriendShipById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách mối quan hệ của một tài khoản theo trạng thái.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách mối quan hệ của tài khoản có `id` theo trạng thái `status`.  
        /// - Nếu `id` không tồn tại trong hệ thống, trả về lỗi `404 Not Found`.  
        /// - Nếu `page` hoặc `size` không hợp lệ, trả về lỗi `400 Bad Request`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của tài khoản cần lấy danh sách mối quan hệ.</param>
        /// <param name="status">Trạng thái mối quan hệ (`None`, `Block`, `Request`, `Requested`, v.v.).</param>
        /// <param name="page">Số trang (mặc định là 1).</param>
        /// <param name="size">Số lượng phần tử trên mỗi trang (mặc định là 10).</param>
        /// <returns>
        /// - `200 OK`: Trả về danh sách mối quan hệ thành công.  
        /// - `400 Bad Request`: `page` hoặc `size` không hợp lệ.  
        /// - `404 Not Found`: Không tìm thấy tài khoản.
        /// </returns>
        [HttpGet(ApiEndPointConstant.FriendShip.GetFriendShipByAccountIdAndStatus)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetFriendShipByAccountIdAndStatus([FromRoute] Guid id, [FromQuery] FriendShipEnum status, [FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _friendShipService.GetFriendShipByAccountIdAndStatus(id, status, pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API lấy danh sách mối quan hệ bạn bè của một tài khoản.
        /// </summary>
        /// <remarks>
        /// - Trả về danh sách mối quan hệ bạn bè của tài khoản có `id`.  
        /// - Chỉ lấy các mối quan hệ có trạng thái `Accepted`.  
        /// - Nếu `id` không tồn tại trong hệ thống, trả về lỗi `404 Not Found`.  
        /// - Nếu `page` hoặc `size` không hợp lệ, trả về lỗi `400 Bad Request`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="id">ID của tài khoản cần lấy danh sách bạn bè.</param>
        /// <param name="page">Số trang (mặc định là 1).</param>
        /// <param name="size">Số lượng phần tử trên mỗi trang (mặc định là 10).</param>
        /// <returns>
        /// - `200 OK`: Trả về danh sách bạn bè thành công.  
        /// - `400 Bad Request`: `page` hoặc `size` không hợp lệ.  
        /// - `404 Not Found`: Không tìm thấy tài khoản.
        /// </returns>
        [HttpGet(ApiEndPointConstant.FriendShip.GetFriendByAccountId)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetFriendByAccountId([FromRoute] Guid id, [FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _friendShipService.GetFriendByAccountId(id, pageNumber, pageSize);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API tìm kiếm bạn bè theo số điện thoại.
        /// </summary>
        /// <remarks>
        /// - Tìm kiếm tài khoản theo số điện thoại `phone`.  
        /// - Chỉ tìm các tài khoản có trạng thái `Active`.  
        /// - Nếu không tìm thấy tài khoản, trả về lỗi `404 Not Found`.  
        /// - Nếu `phone` không hợp lệ hoặc rỗng, trả về lỗi `400 Bad Request`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="phone">Số điện thoại của tài khoản cần tìm kiếm.</param>
        /// <returns>
        /// - `200 OK`: Trả về thông tin tài khoản tìm thấy.  
        /// - `400 Bad Request`: `phone` không hợp lệ.  
        /// - `404 Not Found`: Không tìm thấy tài khoản.
        /// </returns>

        [HttpGet(ApiEndPointConstant.FriendShip.SearchFriendByPhone)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> SearchFriendByPhone([FromQuery] string phone)
        {
            var response = await _friendShipService.SearchFriendByPhone(phone);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API cập nhật thông tin mối quan hệ bạn bè, sử dụng khi accept, hoặc block mối quan hệm,....
        /// </summary>
        /// <remarks>
        /// - Cập nhật thông tin của mối quan hệ bạn bè dựa trên `id`.  
        /// - Nếu `id` không tồn tại, trả về lỗi `404 Not Found`.  
        /// - Nếu cập nhật thành công, trả về `200 OK` cùng dữ liệu mới.  
        /// - Nếu cập nhật thất bại, trả về `400 Bad Request`.
        /// </remarks>
        /// <param name="id">ID của mối quan hệ bạn bè cần cập nhật.</param>
        /// <param name="request">Dữ liệu cập nhật mối quan hệ bạn bè.</param>
        /// <returns>
        /// - `200 OK`: Cập nhật thành công.  
        /// - `400 Bad Request`: Cập nhật thất bại.  
        /// - `404 Not Found`: Không tìm thấy mối quan hệ bạn bè.
        /// </returns>
        [HttpPatch(ApiEndPointConstant.FriendShip.UpdateFriendShip)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateFriendShip([FromRoute] Guid id, [FromBody] UpdateFriendShipRequest request)
        {

            var response = await _friendShipService.UpdateFriendShip(id, request);
            return StatusCode(int.Parse(response.status), response);
        }

        /// <summary>
        /// API xóa mối quan hệ bạn bè.
        /// </summary>
        /// <remarks>
        /// - Đánh dấu mối quan hệ bạn bè là `không hoạt động` thay vì xóa cứng khỏi DB.  
        /// - Nếu `id` không tồn tại, trả về lỗi `404 Not Found`.  
        /// - Nếu xóa thành công, trả về `200 OK`.  
        /// - Nếu xóa thất bại, trả về `400 Bad Request`.
        /// </remarks>
        /// <param name="id">ID của mối quan hệ bạn bè cần xóa.</param>
        /// <returns>
        /// - `200 OK`: Xóa thành công.  
        /// - `400 Bad Request`: Xóa thất bại.  
        /// - `404 Not Found`: Không tìm thấy mối quan hệ bạn bè.
        /// </returns>
        [HttpDelete(ApiEndPointConstant.FriendShip.RemoveFriendShip)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RemoveFriendShip([FromRoute] Guid id)
        {
            var response = await _friendShipService.RemoveFriendShip(id);

            return StatusCode(int.Parse(response.status), response);
        }
    }
}
