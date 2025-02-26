using Galini.Models.Payload.Request.UserPresence;
using Galini.Models.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface IUserPresenceService
    {
        Task<BaseResponse> CreateUserPresence(CreateUserPresenceRequest request, Guid id);
        Task<BaseResponse> UpdateUserPresence(Guid id, UpdateUserPresenceRequest request);
        Task<BaseResponse> RemoveUserPresence(Guid id);
        Task<BaseResponse> GetAllUserPresence(int page, int size);
        Task<BaseResponse> GetUserPresenceById(Guid id);
        Task<BaseResponse> GetUserPresenceByAccountId(Guid id);
    }
}
