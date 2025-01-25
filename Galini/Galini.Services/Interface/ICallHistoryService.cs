using Galini.Models.Payload.Request.CallHistory;
using Galini.Models.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface ICallHistoryService
    {
        public Task<BaseResponse> CreateCallHistory(CreateCallHistoryRequest request);
        public Task<BaseResponse> GetAllCallHistory(int page, int size);
        public Task<BaseResponse> GetCallHistoryById(Guid callHistoryId);
        public Task<BaseResponse> UpdateCallHistory(Guid callHistoryId, CreateCallHistoryRequest request);
        public Task<BaseResponse> RemoveCallHistory(Guid callHistoryId);
    }
}
