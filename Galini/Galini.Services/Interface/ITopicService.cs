using Galini.Models.Payload.Request.Topic;
using Galini.Models.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface ITopicService
    {
        public Task<BaseResponse> CreateTopic(CreateTopicRequest request, Guid listenerInfoId);
        public Task<BaseResponse> GetAllTopic(int page, int size);
        public Task<BaseResponse> GetTopicById(Guid topicId);
        public Task<BaseResponse> GetTopicByListenerInfoId(Guid listenerInfoId);
        public Task<BaseResponse> UpdateTopic(Guid topicId, CreateTopicRequest request);
        public Task<BaseResponse> RemoveTopic(Guid topicId);
    }
}
