using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Payload.Request.ListenerInfo;
using Galini.Models.Payload.Request.Message;
using Galini.Models.Payload.Response;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Implement
{
    public class MessageService : BaseService<MessageService>, IMessageService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public MessageService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<MessageService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public Task<BaseResponse> CreateLMessage(CreateMessageRequest request, Guid directChatId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> CreateLMessageCall(CreateMessageRequest request, Guid directChatId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> GetAllMessage(int page, int size)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> GetMessageByDirectChatId(Guid directChatId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> GetMessageById(Guid messageId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> RemoveMessage(Guid messageId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> SearchMessageByDirectChatId(Guid directChatId, string keyWord)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> UpdateMessage(Guid messageId, CreateMessageRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
