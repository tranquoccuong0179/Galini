using Galini.Models.Request.User;
using Galini.Models.Response;

namespace Galini.Services.Interface;

public interface IUserService
{
    Task<BaseResponse> RegisterUser(RegisterUserRequest request);
}