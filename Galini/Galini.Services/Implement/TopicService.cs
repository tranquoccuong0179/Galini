using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Azure;
using Galini.Models.Entity;
using Galini.Models.Paginate;
using Galini.Models.Payload.Request.Topic;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.Topic;
using Galini.Models.Utils;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Galini.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Galini.Services.Implement
{
    public class TopicService : BaseService<TopicService>, ITopicService
    {
        private readonly IMapper _mapper;
        public TopicService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<TopicService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _mapper = mapper;
        }

        public async Task<BaseResponse> CreateTopic(CreateTopicRequest request)
        {
            Guid? id = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var listenerInfo = await _unitOfWork.GetRepository<ListenerInfo>().SingleOrDefaultAsync(
                predicate: l => l.AccountId.Equals(id) && l.IsActive == true);

            if(listenerInfo == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy người nghe",
                    data = null
                };
            }

            var topicExist = await _unitOfWork.GetRepository<Topic>().SingleOrDefaultAsync(
                predicate: t => t.Name.Equals(request.Name) && t.IsActive == true);
            if(topicExist != null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Chủ đề này đã tồn tại",
                    data = null
                };
            }

            var topic = _mapper.Map<Topic>(request);
            topic.ListenerInfoId = listenerInfo.Id;

            await _unitOfWork.GetRepository<Topic>().InsertAsync(topic);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Thêm chủ đề thành công",
                    data = _mapper.Map<CreateTopicResponse>(topic)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Thêm chủ đề thất bại",
                data = null
            };
        }

        public async Task<BaseResponse> GetAllTopic(int page, int size)
        {
            Guid? id = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var listenerInfo = await _unitOfWork.GetRepository<ListenerInfo>().SingleOrDefaultAsync(
                predicate: l => l.AccountId.Equals(id) && l.IsActive == true);

            if (listenerInfo == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy người nghe",
                    data = null
                };
            }

            var topics = await _unitOfWork.GetRepository<Topic>().GetPagingListAsync(
                selector: t => _mapper.Map<GetTopicResponse>(t),
                predicate: t => t.ListenerInfoId.Equals(listenerInfo.Id) && t.IsActive == true,
                page: page,
                size: size);

            int totalItems = topics.Total;
            int totalPages = (int)Math.Ceiling((double)totalItems / size);

            if (topics == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Danh sách chủ đề",
                    data = new Paginate<Topic>()
                    {
                        Page = page,
                        Size = size,
                        Total = totalItems,
                        TotalPages = totalPages,
                        Items = new List<Topic>()
                    }
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Danh sách chủ đề",
                data = topics
            };


        }

        public async Task<BaseResponse> GetTopicById(Guid id)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var listenerInfo = await _unitOfWork.GetRepository<ListenerInfo>().SingleOrDefaultAsync(
                predicate: l => l.AccountId.Equals(accountId) && l.IsActive == true);

            if (listenerInfo == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy người nghe",
                    data = null
                };
            }

            var topic = await _unitOfWork.GetRepository<Topic>().SingleOrDefaultAsync(
                selector: t => _mapper.Map<GetTopicResponse>(t),
                predicate: t => t.Id.Equals(id) && t.IsActive == true && t.ListenerInfoId.Equals(listenerInfo.Id));

            if(topic == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy chủ đề này",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Chủ đề này",
                data = topic
            };
        }

        public Task<BaseResponse> GetTopicByListenerInfoId(Guid listenerInfoId)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> RemoveTopic(Guid id)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var listenerInfo = await _unitOfWork.GetRepository<ListenerInfo>().SingleOrDefaultAsync(
                predicate: l => l.AccountId.Equals(accountId) && l.IsActive == true);

            if (listenerInfo == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy người nghe",
                    data = null
                };
            }

            var topic = await _unitOfWork.GetRepository<Topic>().SingleOrDefaultAsync(
                predicate: t => t.Id.Equals(id) && t.IsActive == true && t.ListenerInfoId.Equals(listenerInfo.Id));

            if (topic == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy chủ đề này",
                    data = null
                };
            }

            topic.IsActive = false;
            topic.DeleteAt = TimeUtil.GetCurrentSEATime();
            topic.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<Topic>().UpdateAsync(topic);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Xóa chủ đề thành công",
                    data = true
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Xóa chủ đề thất bại",
                data = false
            };
        }

        public async Task<BaseResponse> UpdateTopic(Guid id, UpdateTopicRequest request)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var listenerInfo = await _unitOfWork.GetRepository<ListenerInfo>().SingleOrDefaultAsync(
                predicate: l => l.AccountId.Equals(accountId) && l.IsActive == true);

            if (listenerInfo == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy người nghe",
                    data = null
                };
            }

            var topic = await _unitOfWork.GetRepository<Topic>().SingleOrDefaultAsync(
                predicate: t => t.Id.Equals(id) && t.IsActive == true && t.ListenerInfoId.Equals(listenerInfo.Id));

            if (topic == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy chủ đề này",
                    data = null
                };
            }

            _mapper.Map(request, topic);
            _unitOfWork.GetRepository<Topic>().UpdateAsync(topic);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Cập nhật chủ đề thành công",
                    data = _mapper.Map<GetTopicResponse>(topic)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Cập nhật chủ đề thất bại",
                data = null
            };
        }
    }
}
