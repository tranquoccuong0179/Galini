using Galini.Models.Enum;
using Galini.Models.Payload.Request.WorkShift;
using Galini.Models.Payload.Response;
using Microsoft.AspNetCore.Mvc;
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
        Task<BaseResponse> GetAvailableWorkShifts(int page, int size, Guid id, DateTime date);
        Task<BaseResponse> GetAllWorkShift(int page, int size);
        Task<BaseResponse> CreateWorkShift(CreateWorkShiftRequest request, Guid accountId, DayEnum day);
        Task<BaseResponse> UpdateWorkShift(UpdateWorkShiftRequest request, Guid id, DayEnum day);
        Task<BaseResponse> RemoveWorkShift(Guid id);
    }
}
