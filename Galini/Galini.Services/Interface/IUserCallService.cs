using Galini.Models.Payload.Request.UserCall;
using Galini.Models.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface IUserCallService
    {
        public Task<BaseResponse> CreateUserCall(CreateUserCallRequest request, Guid accountId, Guid callHistoryId);
        public Task<BaseResponse> GetAllUserCall(int page, int size);
        public Task<BaseResponse> GetUserCallById(Guid userCallId);
        public Task<BaseResponse> GetUserCallByAccountId(Guid accountId);
        public Task<BaseResponse> UpdateUserCall(Guid userCallId, CreateUserCallRequest request);
        public Task<BaseResponse> RemoveUserCall(Guid userCallId);
    }
}
