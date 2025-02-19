using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galini.Models.Payload.Request.Authenticaion;
using Galini.Models.Payload.Response;

namespace Galini.Services.Interface
{
    public interface IAuthService
    {
        Task<BaseResponse> Authenticate(AuthenticateRequest request);
        Task<BaseResponse> AutheticateWithRefreshToken(string refreshTokenRequest);
    }
}
