using Galini.Models.Payload.Request.ListenerInfo;
using Galini.Models.Payload.Request.User;
using Galini.Models.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface IListenerInfoService
    {
        public Task<BaseResponse> CreateListenerInfo(RegisterUserRequest registerRequest, CreateListenerInfoRequest request);
        public Task<BaseResponse> GetAllListenerInfo(int page, int size);
        public Task<BaseResponse> GetListenerInfoById(Guid listenerInfoId);
        public Task<BaseResponse> GetListenerInfoByAccountId(Guid accountId);
        public Task<BaseResponse> UpdateListenerInfo(Guid listenerInfoId, CreateListenerInfoRequest request);
        public Task<BaseResponse> RemoveListenerInfo(Guid listenerInfoId);
    }
}
