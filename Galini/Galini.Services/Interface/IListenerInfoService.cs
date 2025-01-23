using Galini.Models.Request.ListenerInfo;
using Galini.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface IListenerInfoService
    {
        public Task<BaseResponse> CreateListenerInfo(CreateListenerInfoRequest request, Guid accountId);
        public Task<BaseResponse> GetAllListenerInfo(int page, int size);
        public Task<BaseResponse> GetListenerInfoById(Guid friendShipId);
        public Task<BaseResponse> UpdateListenerInfo(Guid friendShipId, CreateListenerInfoRequest request);
        public Task<BaseResponse> RemoveListenerInfo(Guid friendShipId);
    }
}
