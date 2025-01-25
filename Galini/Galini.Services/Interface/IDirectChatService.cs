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
        public Task<BaseResponse> CreateDirectChat();
        public Task<BaseResponse> GetAllDirectChat(int page, int size);
        public Task<BaseResponse> GetDirectChatById(Guid directChatId);
        public Task<BaseResponse> UpdateDirectChat(Guid directChatId);
        public Task<BaseResponse> RemoveDirectChat(Guid directChatId);
    }
}
