using Galini.Models.Request.ListenerInfo;
using Galini.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface IMessageService
    {
        public Task<BaseResponse> CreateLMessage(CreateListenerInfoRequest request, Guid directChatId);
        public Task<BaseResponse> GetAllMessage(int page, int size);
        public Task<BaseResponse> GetMessageById(Guid messageId);
        public Task<BaseResponse> GetMessageByDirectChatId(Guid directChatId);
        public Task<BaseResponse> SearchMessageByDirectChatId(Guid directChatId, string keyWord);
        public Task<BaseResponse> UpdateMessage(Guid messageId, CreateListenerInfoRequest request);
        public Task<BaseResponse> RemoveMessage(Guid messageId);
    }
}
