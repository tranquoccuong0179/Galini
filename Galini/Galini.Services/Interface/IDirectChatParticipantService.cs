using Galini.Models.Payload.Request.Deposit;
using Galini.Models.Payload.Request.DirectChatParticipant;
using Galini.Models.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface IDirectChatParticipantService
    {
        public Task<BaseResponse> CreateDirectChatParticipant(CreateDirectChatParticipant request);
        public Task<BaseResponse> GetAllDirectChatParticipant(int page, int size);
        public Task<BaseResponse> GetDirectChatParticipantById(Guid id);
        //public Task<BaseResponse> GetDirectChatParticipantByAccountId(Guid accountId);
        public Task<BaseResponse> UpdateDirectChatParticipant(Guid id, UpdateDirectChatParticipant request);
        public Task<BaseResponse> RemoveDirectChatParticipant(Guid id);
        //public Task<BaseResponse> GetAllDirectChatByAccountId(Guid accountId);
    }
}
