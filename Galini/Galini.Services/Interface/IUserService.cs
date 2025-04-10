using Galini.Models.Payload.Request.User;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.GoogleAuthentication;

namespace Galini.Services.Interface;

public interface IUserService
{
    Task<BaseResponse> RegisterUser(RegisterUserRequest request);
    Task<BaseResponse> VerifyOtp(string email, string otp);
    Task<BaseResponse> ResendOtp(string email);
    Task<GoogleAuthResponse> CreateTokenByEmail(string email);
    Task<bool> GetAccountByEmail(string email);
    Task<BaseResponse> CreateNewUserAccountByGoogle(GoogleAuthResponse request);
    Task<BaseResponse> GetListenerAccount(int page, int size);
    Task<BaseResponse> UpdateDuration(Guid id, int duration);
    Task<BaseResponse> GetAccountById(Guid id);
    Task<BaseResponse> GetFriendById(Guid id);
    Task<BaseResponse> GetAllUser(int page, int size);
    Task<BaseResponse> DeleteUser(Guid id);
}