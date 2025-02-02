using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Messaging;
using Galini.Models.Entity;
using Galini.Models.Enum;
using Galini.Models.Payload.Request.Authenticaion;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.Authentication;
using Galini.Models.Utils;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Galini.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Galini.Services.Implement
{
    public class AuthService : BaseService<AuthService>, IAuthService
    {
        public AuthService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<AuthService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse> Authenticate(AuthenticateRequest request)
        {
            Expression<Func<Account, bool>> searchFilter = p =>
                  p.UserName.Equals(request.Username) &&
                  p.Password.Equals(PasswordUtil.HashPassword(request.Password)) &&
                  (p.Role == RoleEnum.Customer.GetDescriptionFromEnum()||
                  p.Role == RoleEnum.Listener.GetDescriptionFromEnum());
            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(predicate: searchFilter);
            if (account == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Tài khoản hoặc mật khẩu không đúng",
                    data = null
                };
            }

            RoleEnum role = EnumUtil.ParseEnum<RoleEnum>(account.Role);
            Tuple<string, Guid> guildClaim = new Tuple<string, Guid>("accountId", account.Id);
            var token = JwtUtil.GenerateJwtToken(account, guildClaim);

            // Create the login response object
            var authenticateResponse = new AuthenticateResponse()
            {
                RoleEnum = role.ToString(),
                AccountId = account.Id,
                UserName = account.UserName,
                token = token // Assign the generated token
            };

            // Return a success response
            return new BaseResponse
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Login successful.",
                data = authenticateResponse
            };
        }
    }
}
