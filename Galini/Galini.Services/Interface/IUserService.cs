using Galini.Models.Payload.Request.User;
using Galini.Models.Payload.Response;

namespace Galini.Services.Interface;

public interface IUserService
{
    Task<BaseResponse> RegisterUser(RegisterUserRequest request);
}