using Galini.Models.Request.Deposit;
using Galini.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface IDirectChatParticipantService
    {
        public Task<BaseResponse> CreateDirectChatParticipant(CreateDepositRequest request, Guid accountId, Guid directChatId);
        public Task<BaseResponse> GetAllDirectChatParticipant(int page, int size);
        public Task<BaseResponse> GetDirectChatParticipantById(Guid directChatParticipantId);
        public Task<BaseResponse> GetDirectChatParticipantByAccountId(Guid accountId);
        public Task<BaseResponse> UpdateDirectChatParticipant(Guid directChatParticipantId, CreateDepositRequest request);
        public Task<BaseResponse> RemoveDirectChatParticipant(Guid directChatParticipantId);
        public Task<BaseResponse> GetAllDirectChatByAccountId(Guid accountId);
    }
}
