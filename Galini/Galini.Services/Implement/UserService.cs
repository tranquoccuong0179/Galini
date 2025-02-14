using System.Text.RegularExpressions;
using AutoMapper;
using Azure.Messaging;
using Galini.Models.Entity;
using Galini.Models.Enum;
using Galini.Models.Payload.Request.User;
using Galini.Models.Payload.Request.UserInfo;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.Account;
using Galini.Models.Payload.Response.GoogleAuthentication;
using Galini.Models.Utils;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Galini.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MimeKit.Utils;
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

        string otp = OtpUtil.GenerateOtp();

        string maskedUsername = MaskUsername(request.UserName);

        await SendOtpEmail(request.Email, otp, maskedUsername);

        var account = _mapper.Map<Account>(request);

        await _unitOfWork.GetRepository<Account>().InsertAsync(account);
        bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;
        var redisDb = _redis.GetDatabase();
        if (redisDb == null) throw new RedisServerException("Không thể kết nối tới Redis");
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

    public async Task<BaseResponse> ResendOtp(string email)
    {
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(email, emailPattern))
        {
            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Email không đúng định dạng",
                data = false
            };
        }

        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
        predicate: u => u.Email.Equals(email));
        if (account == null)
        {
            return new BaseResponse()
            {
                status = StatusCodes.Status404NotFound.ToString(),
                message = "Email không tồn tại trong hệ thống",
                data = null
            };
        }

        var redisDb = _redis.GetDatabase();
        if (redisDb == null)
            throw new RedisServerException("Không thể kết nối tới Redis");

        var key = "emailOtp:" + email;

        await redisDb.KeyDeleteAsync(key);

        string maskedUsername = MaskUsername(account.UserName);

        string otp = OtpUtil.GenerateOtp();

        await SendOtpEmail(email, otp, maskedUsername);

        await redisDb.StringSetAsync(key, otp, TimeSpan.FromMinutes(5));

        return new BaseResponse()
        {
            status = StatusCodes.Status200OK.ToString(),
            message = "Gửi lại OTP thành công",
            data = true
        };


    }

    public async Task<BaseResponse> VerifyOtp(string email, string otp)
    {
        var redisDb = _redis.GetDatabase();
        if (redisDb == null) throw new RedisServerException("Không thể kết nối tới Redis");

        var key = "emailOtp:" + email;
        var storedOtp = await redisDb.StringGetAsync(key);
        if (storedOtp.IsNullOrEmpty)
        {
            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "OTP đã hết hạn hoặc không tồn tại",
                data = null
            };
        }

        if (!storedOtp.Equals(otp))
        {
            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "OTP không chính xác",
                data = null
            };
        }

        var account = await _unitOfWork.GetRepository<Account>()
            .SingleOrDefaultAsync(predicate: a => a.Email.Equals(email));
        if (account == null)
        {
            return new BaseResponse()
            {
                status = StatusCodes.Status404NotFound.ToString(),
                message = "Tài khoản không tồn tại",
                data = null
            };
        }

        account.IsActive = true;
        var wallet = _mapper.Map<Wallet>(account);
        await _unitOfWork.GetRepository<Wallet>().InsertAsync(wallet);
        _unitOfWork.GetRepository<Account>().UpdateAsync(account);
        await _unitOfWork.CommitAsync();

        await redisDb.KeyDeleteAsync(key);

        return new BaseResponse()
        {
            status = StatusCodes.Status200OK.ToString(),
            message = "Tài khoản đã được xác thực thành công",
            data = _mapper.Map<RegisterUserResponse>(account)
        };
    }

    private string MaskUsername(string username)
    {
        if (username.Length <= 2) return username;
        return username[0] + new string('*', username.Length - 2) + username[^1];
    }

    private async Task SendOtpEmail(string email, string otp, string username)
    {
        try
        {
            // Tạo subject cho email
            var subject = "Welcome to Harmon";

            // Tạo nội dung email, bao gồm OTP
            var messageBody = $"{otp}";

            // Gọi hàm SendEmailAsync từ _emailSender (đã được cấu hình trước đó)
            await _emailService.SendEmailAsync(email, subject, messageBody, username);
        }
        catch (Exception ex)
        {
            // Xử lý lỗi khi gửi email
            Console.WriteLine($"Error sending OTP email: {ex.Message}");
        }
    }

    public async Task<string> CreateTokenByEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("Username cannot be null or empty", nameof(email));
        }
        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: p => p.Email.Equals(email)
        );
        if (account == null) throw new BadHttpRequestException("Account not found");
        var guidClaim = new Tuple<string, Guid>("userId", account.Id);
        var token = JwtUtil.GenerateJwtToken(account, guidClaim);
        return token;
    }

    public async Task<bool> GetAccountByEmail(string email)
    {
        if (email == null) throw new BadHttpRequestException("Email cannot be null");

        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: p => p.Email.Equals(email)
        );
        return account != null;
    }
    public async Task<BaseResponse> CreateNewUserAccountByGoogle(GoogleAuthResponse request)
    {
        var existingUser = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: u => u.Email.Equals(request.Email) &&
                            u.IsActive == true);

        if (existingUser != null)
        {
            return new BaseResponse
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "User account already exists.",
                data = new RegisterUserResponse
                {
                    Email = existingUser.Email,
                    FullName = existingUser.FullName
                }
            };
        }

        var newUser = new Account()
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            FullName = request.FullName,
            UserName = request.Email.Split("@")[0],
            Role = RoleEnum.Customer.GetDescriptionFromEnum(),
            IsActive = true,
            Password = PasswordUtil.HashPassword("12345678"),
            Phone = "0000000000",
            DateOfBirth = TimeUtil.GetCurrentSEATime(),
            Gender = GenderEnum.Male.GetDescriptionFromEnum(),
            AvatarUrl = string.Empty,
            CreateAt = TimeUtil.GetCurrentSEATime(),
            UpdateAt = TimeUtil.GetCurrentSEATime(),
        };

        await _unitOfWork.GetRepository<Account>().InsertAsync(newUser);
        await _unitOfWork.CommitAsync();

        return new BaseResponse
        {
            status = StatusCodes.Status201Created.ToString(),
            message = "User account created successfully.",
            data = new RegisterUserResponse
            {
                Email = newUser.Email,
                FullName = newUser.FullName
            }
        };
    }

}