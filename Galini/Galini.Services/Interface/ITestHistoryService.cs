using Galini.Models.Payload.Request.TestHistory;
using Galini.Models.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface ITestHistoryService
    {
        public Task<BaseResponse> CreateTestHistory(CreateTestHistoryRequest request);
        public Task<BaseResponse> GetAllTestHistory(int page, int size, int? grade, string? status, bool? sortByGrade);
        public Task<BaseResponse> GetTestHistoryById(Guid testHistoryId);
        public Task<BaseResponse> GetTestHistoryByAccountId(int page, int size, int? grade, string? status, bool? sortByGrade);
        public Task<BaseResponse> UpdateTestHistory(Guid testHistoryId, UpdateTestHistoryRequest request);
        public Task<BaseResponse> RemoveTestHistory(Guid testHistoryId);
    }
}
