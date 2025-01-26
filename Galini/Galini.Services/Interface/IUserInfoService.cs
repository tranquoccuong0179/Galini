using Galini.Models.Payload.Request.UserCall;
using Galini.Models.Payload.Request.UserInfo;
using Galini.Models.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface IUserInfoService
    {
        public Task<BaseResponse> CreateUserInfo(CreateUserInfoRequest request, Guid accountId, Guid premiumId);
        public Task<BaseResponse> GetAllUserInfo(int page, int size);
        public Task<BaseResponse> GetUserInfoById(Guid userInfoId);
        public Task<BaseResponse> GetUserInfoByAccountId(Guid accountId);
        public Task<BaseResponse> UpdateUserInfo(Guid userInfoId, Guid premiumId, UpdateUserInfoRequest request);
        public Task<BaseResponse> RemoveUserInfo(Guid userInfoId);
    }
}
