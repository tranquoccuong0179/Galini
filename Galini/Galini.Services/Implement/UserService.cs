using System.Text.RegularExpressions;
using AutoMapper;
using Azure.Messaging;
using Galini.Models.Entity;
using Galini.Models.Enum;
using Galini.Models.Payload.Request.User;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.Account;
using Galini.Models.Utils;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Galini.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Galini.Services.Implement;

public class UserService : BaseService<UserService>, IUserService
{
    
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly IConnectionMultiplexer _redis;
    private readonly IEmailService _emailService;
    public UserService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<UserService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmailService emailService, IConnectionMultiplexer redis) : base(unitOfWork, logger, mapper, httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _emailService = emailService;
        _redis = redis;
    }

    public async Task<BaseResponse> RegisterUser(RegisterUserRequest request)
    {
        var ipAddress = _httpContextAccessor.HttpContext?.Request?.Headers["X-Forwarded-For"].FirstOrDefault()
                        ?? _httpContextAccessor.HttpContext?.Request?.Headers["X-Real-IP"].FirstOrDefault()
                        ?? _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(request.Email, emailPattern))
        {
            return new BaseResponse()
            {   
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Email không đúng định dạng",
                data = null
            };
        }
        
        string phonePattern = @"^0\d{9}$";
        if (!Regex.IsMatch(request.Phone, phonePattern))
        {
            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Số điện thoại không đúng định dạng",
                data = null
            };
        }

        var isUserNameExist = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: u => u.UserName.Equals(request.UserName));
        if (isUserNameExist != null)
        {
            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Username đã tồn tại",
                data = null
            };
        }

        var isEmailExist = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: u => u.Email.Equals(request.Email));
        if (isEmailExist != null)
        {
            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Email đã tồn tại",
                data = null
            };
        }
        
        var isPhoneExist = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: u => u.Phone.Equals(request.Phone));
        if (isPhoneExist != null)
        {
            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Số điện thoại đã tồn tại",
                data = null
            };
        }


        //var isIPExist = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
        //    predicate: u => u.IdentifyIp.Equals(ipAddress));
        //if (isIPExist != null)
        //{
        //    return new BaseResponse()
        //    {
        //        status = StatusCodes.Status400BadRequest.ToString(),
        //        message = "Địa chỉ IP đã tồn tại",
        //        data = null
        //    };
        //}

        //Account account = new Account()
        //{
        //    Id = Guid.NewGuid(),
        //    UserName = request.UserName,
        //    Email = request.Email,
        //    Password = PasswordUtil.HashPassword(request.Password),
        //    Role = RoleEnum.Customer.GetDescriptionFromEnum(),
        //    Gender = request.Gender.GetDescriptionFromEnum(),
        //    FullName = request.FullName,
        //    DateOfBirth = request.DateOfBirth,
        //    Phone = request.Phone,
        //    //Weight = request.Weight,
        //    AvatarUrl = request.AvatarUrl,
        //    //IdentifyIp = ipAddress,
        //    IsActive = true,
        //    CreateAt = TimeUtil.GetCurrentSEATime(),
        //    UpdateAt = TimeUtil.GetCurrentSEATime()
        //};

        string otp = OtpUtil.GenerateOtp();

        await SendOtpEmail(request.Email, otp);

        var account = _mapper.Map<Account>(request);

        await _unitOfWork.GetRepository<Account>().InsertAsync(account);
        bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;
        var redisDb = _redis.GetDatabase();
        if (redisDb == null) throw new RedisServerException("");
        var key = "emailOtp:" + request.Email;
        await redisDb.StringSetAsync(key, otp, TimeSpan.FromMinutes(5));
        if (isSuccessfully)
        {
            return new BaseResponse()
            {
                status = StatusCodes.Status201Created.ToString(),
                message = "Đăng kí tài khoản thành công",
                data = _mapper.Map<RegisterUserResponse>(account)
            };
        }

        return new BaseResponse()
        {
            status = StatusCodes.Status400BadRequest.ToString(),
            message = "Đăng kí tài khoản thất bại",
            data = null
        };
    }
    private async Task SendOtpEmail(string email, string otp)
    {
        try
        {
            // Tạo subject cho email
            var subject = "Welcome to Harmon";

            // Tạo nội dung email, bao gồm OTP
            var messageBody = $"Your OTP code is: {otp}. This code is valid for 5 minutes.";

            // Gọi hàm SendEmailAsync từ _emailSender (đã được cấu hình trước đó)
            await _emailService.SendEmailAsync(email, subject, messageBody);
        }
        catch (Exception ex)
        {
            // Xử lý lỗi khi gửi email
            Console.WriteLine($"Error sending OTP email: {ex.Message}");
        }
    }
}