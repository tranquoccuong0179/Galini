using Galini.Models.Payload.Request.WorkShift;
using Galini.Models.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface IWorkShiftService
    {
        Task<BaseResponse> GetWorkShiftById(Guid id);
        Task<BaseResponse> GetWorkShiftByAccountId(int page, int size, Guid id);
        Task<BaseResponse> GetAllWorkShift(int page, int size);
        Task<BaseResponse> CreateWorkShift(CreateWorkShiftRequest request, Guid accountId);
        Task<BaseResponse> UpdateWorkShift(UpdateWorkShiftRequest request, Guid id);
        Task<BaseResponse> RemoveWorkShift(Guid id);
    }
}
