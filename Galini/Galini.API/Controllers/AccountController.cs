using Galini.API.Constants;
using Galini.Models.Payload.Request.User;
using Galini.Models.Payload.Response;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers;

public class AccountController : BaseController<AccountController>
{
    private readonly IUserService _userService;
    public AccountController(ILogger<AccountController> logger, IUserService userService) : base(logger)
    {
        _userService = userService;
    }

    /// <summary>
    /// API tạo đăng ký tài khoản mới cho người dùng.
    /// </summary>
    /// <remarks>
    /// - Yêu cầu thông tin người dùng thông qua `RegisterUserRequest`.
    /// - Trả về `BaseResponse` chứa kết quả đăng ký.
    /// </remarks>
    /// <param name="request">Thông tin đăng ký của người dùng.</param>
    /// <returns>
    /// - `200 OK`: Đăng ký thành công.  
    /// - `400 Bad Request`: Thông tin không hợp lệ.
    /// </returns>
    [HttpPost(ApiEndPointConstant.User.RegisterUser)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    public async Task<IActionResult> CreateNewAccount([FromBody] RegisterUserRequest request)
    {
        var response = await _userService.RegisterUser(request);
        return StatusCode(int.Parse(response.status), response);
    }

    /// <summary>
    /// API xác thực tài khoản người dùng mới đăng ký.
    /// </summary>
    /// <remarks>
    /// - Yêu cầu thông tin người dùng thông qua `VerifyOtpRequest`.
    /// - Trả về `BaseResponse` chứa kết quả đăng ký.
    /// - Nếu xác thực thất bại, trả về lỗi phù hợp.
    /// </remarks>
    /// <param name="request">Thông tin xác thực OTP, bao gồm email và mã OTP.</param>
    /// <returns>
    /// - `200 OK`: Đăng ký thành công.  
    /// - `400 Bad Request`: Thông tin không hợp lệ.
    /// - `404 Not Found`: Không tìm thấy người dùng hợp lệ.
    /// </returns>
    [HttpPost(ApiEndPointConstant.User.VerifyOtp)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
    {
        var response = await _userService.VerifyOtp(request.Email, request.Otp);
        return StatusCode(int.Parse(response.status), response);
    }

    /// <summary>
    /// API gửi lại mã OTP để xác thực tài khoản người dùng.
    /// </summary>
    /// <remarks>
    /// - Yêu cầu địa chỉ email của người dùng để gửi lại OTP.  
    /// - Trả về `BaseResponse` chứa kết quả gửi OTP.  
    /// - Nếu email không hợp lệ hoặc không tồn tại, trả về lỗi phù hợp.  
    /// </remarks>
    /// <param name="email">Địa chỉ email của người dùng cần nhận lại mã OTP.</param>
    /// <returns>
    /// - `200 OK`: Mã OTP đã được gửi lại thành công.  
    /// - `400 Bad Request`: Email không hợp lệ hoặc không thể gửi OTP.  
    /// - `404 Not Found`: Không tìm thấy tài khoản tương ứng với email.
    /// </returns>
    [HttpPost(ApiEndPointConstant.User.ResendOtp)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    public async Task<IActionResult> ResendOtp([FromBody] string email)
    {
        var response = await _userService.ResendOtp(email);
        return StatusCode(int.Parse(response.status), response);
    }

    /// <summary>
    /// API lấy danh sách tài khoản người nghe với phân trang.
    /// </summary>
    /// <remarks>
    /// - Trả về danh sách tài khoản người nghe có hỗ trợ phân trang.
    /// - Nếu không truyền `page` hoặc `size`, giá trị mặc định sẽ được sử dụng (`page = 1`, `size = 10`).  
    /// - Kết quả trả về được bọc trong `BaseResponse`.
    /// </remarks>
    /// <param name="page">Số trang hiện tại (mặc định là 1).</param>
    /// <param name="size">Số lượng tài khoản trên mỗi trang (mặc định là 10).</param>
    /// <returns>
    /// - `200 OK`: Trả về danh sách tài khoản người nghe thành công.  
    /// - `400 Bad Request`: Tham số phân trang không hợp lệ.  
    /// </returns>
    [HttpGet(ApiEndPointConstant.User.GetListenerAccount)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    public async Task<IActionResult> GetListenerAccount([FromQuery] int? page, [FromQuery] int? size)
    {
        int pageNumber = page ?? 1;
        int pageSize = size ?? 10;
        var response = await _userService.GetListenerAccount(pageNumber, pageSize);

        return StatusCode(int.Parse(response.status), response);
    }

    /// <summary>
    /// API cập nhật giá trị Duration của tài khoản người dùng.
    /// </summary>
    /// <remarks>
    /// - Cập nhật giá trị `Duration` cho tài khoản người dùng dựa trên `id`.  
    /// - Người dùng phải có trạng thái hoạt động (`IsActive = true`) mới có thể cập nhật.  
    /// - Kết quả trả về được bọc trong `BaseResponse`.
    /// </remarks>
    /// <param name="id">ID của người dùng cần cập nhật.</param>
    /// <param name="duration">Giá trị `Duration` mới cần thiết lập.</param>
    /// <returns>
    /// - `200 OK`: Cập nhật `Duration` thành công.  
    /// - `400 Bad Request`:  
    ///   - Người dùng không tồn tại.  
    ///   - Cập nhật `Duration` thất bại.  
    /// </returns>
    [HttpPatch(ApiEndPointConstant.User.UpdateDuration)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    public async Task<IActionResult> UpdateDuration([FromRoute] Guid id, [FromQuery] int duration)
    {
        var response = await _userService.UpdateDuration(id, duration);

        return StatusCode(int.Parse(response.status), response);
    }

    /// <summary>
    /// API lấy thông tin tài khoản người dùng theo ID.
    /// </summary>
    /// <remarks>
    /// - Trả về thông tin chi tiết của tài khoản dựa trên `id` cung cấp.  
    /// - Kết quả trả về được bọc trong `BaseResponse`.
    /// </remarks>
    /// <param name="id">ID của tài khoản cần lấy thông tin.</param>
    /// <returns>
    /// - `200 OK`: Trả về thông tin tài khoản thành công.  
    /// - `400 Bad Request`: Không tìm thấy tài khoản hoặc ID không hợp lệ.  
    /// </returns>
    [HttpGet(ApiEndPointConstant.User.GetAccountById)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    public async Task<IActionResult> GetAccountById([FromRoute] Guid id)
    {
        var response = await _userService.GetAccountById(id);

        return StatusCode(int.Parse(response.status), response);
    }

    /// <summary>
    /// API lấy thông tin tài khoản người dùng theo ID.
    /// </summary>
    /// <remarks>
    /// - Trả về thông tin chi tiết của tài khoản dựa trên `id` cung cấp.  
    /// - Kết quả trả về được bọc trong `BaseResponse`.
    /// </remarks>
    /// <param name="id">ID của tài khoản cần lấy thông tin.</param>
    /// <returns>
    /// - `200 OK`: Trả về thông tin tài khoản thành công.  
    /// - `400 Bad Request`: Không tìm thấy tài khoản hoặc ID không hợp lệ.  
    /// </returns>
    [HttpGet(ApiEndPointConstant.User.GetFriendById)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    public async Task<IActionResult> GetFriendById([FromRoute] Guid id)
    {
        var response = await _userService.GetFriendById(id);

        return StatusCode(int.Parse(response.status), response);
    }
}