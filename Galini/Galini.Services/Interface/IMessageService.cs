using Galini.Models.Payload.Request.ListenerInfo;
using Galini.Models.Payload.Request.Message;
using Galini.Models.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface IMessageService
    {
        public Task<BaseResponse> CreateMessage(CreateMessageRequest request, Guid directChatId);
        public Task<BaseResponse> CreateMessageCall(CreateMessageRequest request, Guid directChatId);
        public Task<BaseResponse> GetAllMessage(int page, int size);
        public Task<BaseResponse> GetMessageById(Guid messageId);
        public Task<BaseResponse> GetMessageByDirectChatId(Guid directChatId, DateTime? beforeCursor);
        public Task<BaseResponse> SearchMessageByDirectChatId(Guid directChatId, string keyWord, int page, int size);
        public Task<BaseResponse> UpdateMessage(Guid messageId, UpdateMessageRequest request);
        public Task<BaseResponse> RemoveMessage(Guid messageId);
    }
}
