using Galini.Models.Request.TestHistory;
using Galini.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface ITestHistoryService
    {
        public Task<BaseResponse> CreateTestHistory(CreateTestHistoryRequest request, Guid accountId);
        public Task<BaseResponse> GetAllTestHistory(int page, int size);
        public Task<BaseResponse> GetTestHistoryById(Guid testHistoryId);
        public Task<BaseResponse> GetTestHistoryByAccountId(Guid accountId);
        public Task<BaseResponse> UpdateTestHistory(Guid testHistoryId, CreateTestHistoryRequest request);
        public Task<BaseResponse> RemoveTestHistory(Guid testHistoryId);
    }
}
