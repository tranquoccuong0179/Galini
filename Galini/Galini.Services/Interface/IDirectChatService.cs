using Galini.Models.Payload.Request.DirectChat;
using Galini.Models.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface IDirectChatService
    {
        public Task<BaseResponse> CreateDirectChat(CreateDirectChatRequest request);
        public Task<BaseResponse> GetAllDirectChat(int page, int size);
        public Task<BaseResponse> GetDirectChatById(Guid id);
        public Task<BaseResponse> UpdateDirectChat(Guid id, UpdateDirectChatRequest request);
        public Task<BaseResponse> RemoveDirectChat(Guid id);
    }
}
