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
using Galini.Models.Payload.Response.ListenerInfo;
using Galini.Models.Utils;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Galini.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
                  p.Role == RoleEnum.Listener.GetDescriptionFromEnum() ||
                  p.Role == RoleEnum.Admin.GetDescriptionFromEnum());
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

            var refreshToken = new RefreshToken()
            {
                Id = Guid.NewGuid(),
                UserId = account.Id,
                Token = JwtUtil.GenerateRefreshToken(),
                ExpirationTime = DateTime.Now.AddDays(30),
            };

            await _unitOfWork.GetRepository<RefreshToken>().InsertAsync(refreshToken);

            // Create the login response object
            var authenticateResponse = new AuthenticateResponse()
            {
                RoleEnum = role.ToString(),
                AccountId = account.Id,
                UserName = account.UserName,
                token = token, // Assign the generated token
                RefreshToken = refreshToken.Token
            };

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                // Return a success response
                return new BaseResponse
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Login successful.",
                    data = authenticateResponse
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Login failed",
                data = null
            };
        }

        public async Task<BaseResponse> AutheticateWithRefreshToken(string refreshTokenRequest)
        {
            RefreshToken? refreshToken = await _unitOfWork.GetRepository<RefreshToken>().SingleOrDefaultAsync(
                predicate: r => r.Token == refreshTokenRequest,
                include: r => r.Include(r => r.User)
                );

            if (refreshToken == null || refreshToken.ExpirationTime < DateTime.UtcNow)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "The refresh token has expired",
                    data = null
                };
            }

            refreshToken.Token = JwtUtil.GenerateRefreshToken();
            refreshToken.ExpirationTime = DateTime.Now.AddDays(30);

            _unitOfWork.GetRepository<RefreshToken>().UpdateAsync(refreshToken);

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccessfully)
            {
                // Return a success response
                return new BaseResponse()
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Login failed",
                    data = null
                };
            }

            RoleEnum role = EnumUtil.ParseEnum<RoleEnum>(refreshToken.User.Role);
            Tuple<string, Guid> guildClaim = new Tuple<string, Guid>("accountId", refreshToken.User.Id);
            var token = JwtUtil.GenerateJwtToken(refreshToken.User, guildClaim);

            // Create the login response object
            var authenticateResponse = new AuthenticateResponse()
            {
                RoleEnum = role.ToString(),
                AccountId = refreshToken.User.Id,
                UserName = refreshToken.User.UserName,
                token = token, // Assign the generated token
                RefreshToken = refreshToken.Token
            };
              
            return new BaseResponse
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Login successful.",
                data = authenticateResponse
            };           
        }
    }
}
