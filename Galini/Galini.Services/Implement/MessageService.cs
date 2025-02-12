using AutoMapper;
using Azure;
using Galini.Models.Entity;
using Galini.Models.Enum;
using Galini.Models.Payload.Request.Message;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.CallHistory;
using Galini.Models.Payload.Response.Message;
using Galini.Models.Utils;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Galini.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public async Task<BaseResponse> CreateMessage(CreateMessageRequest request, Guid directChatId)
        {
            var directChat = await _unitOfWork.GetRepository<DirectChat>().SingleOrDefaultAsync(
                predicate: dc => dc.Id.Equals(directChatId) && dc.IsActive);

            if (directChat == null)
            {
                _logger.LogWarning($"Không tìm thấy phòng chat có Id {directChatId} .");
                return new BaseResponse() 
                { 
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Phòng chat không tồn tại",
                    data = null                    
                };

            }

            Guid? id = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id) && x.IsActive);
            if (account == null)
            {
                _logger.LogWarning($"Không tìm thấy tài khoản có Id {id} .");
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Tài khoản không tồn tại",
                    data = null
                };
            }

            var message = _mapper.Map<CreateMessageRequest, Message>(request);
            message.DirectChatId = directChatId;
            message.SenderId = account.Id;
            message.Type = MessageEnum.Message.ToString();

            await _unitOfWork.GetRepository<Message>().InsertAsync(message);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Tạo tin nhắn thành công",
                    data = _mapper.Map<CreateMessageResponse>(message)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Tạo tin nhắn thất bại",
                data = null
            };
        }

        public async Task<BaseResponse> CreateMessageCall(CreateMessageRequest request, Guid directChatId)
        {
            var directChat = await _unitOfWork.GetRepository<DirectChat>().SingleOrDefaultAsync(
                predicate: dc => dc.Id.Equals(directChatId) && dc.IsActive);

            if (directChat == null)
            {
                _logger.LogWarning($"Không tìm thấy phòng chat có Id {directChatId} .");
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Phòng chat không tồn tại",
                    data = null
                };
            }

            Guid? id = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id) && x.IsActive);
            if (account == null)
            {
                _logger.LogWarning($"Không tìm thấy tài khoản có Id {id} .");
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Tài khoản không tồn tại",
                    data = null
                };
            }

            var message = _mapper.Map<CreateMessageRequest, Message>(request);
            message.DirectChatId = directChatId;
            message.SenderId = account.Id;
            message.Type = MessageEnum.Call.ToString();

            await _unitOfWork.GetRepository<Message>().InsertAsync(message);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Tạo tin nhắn thành công",
                    data = _mapper.Map<CreateMessageResponse>(message)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Tạo tin nhắn thất bại",
                data = null
            };
        }

        public async Task<BaseResponse> GetAllMessage(int page, int size)
        {
            if (page < 1 || size < 1)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Page hoặc size không hợp lệ.",
                    data = null
                };
            }

            var messages = await _unitOfWork.GetRepository<Message>().GetPagingListAsync(
                selector: a => _mapper.Map<CreateMessageResponse>(a),
                predicate: a => a.IsActive,
                page: page,
                size: size);

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy tin nhắn thành công",
                data = messages
            };
        }

        public async Task<BaseResponse> GetMessageByDirectChatId(Guid directChatId, DateTime? beforeCursor)
        {
            var directChat = await _unitOfWork.GetRepository<DirectChat>().SingleOrDefaultAsync(
                predicate: x => x.IsActive && x.Id.Equals(directChatId));

            if (directChat == null)
            {
                _logger.LogWarning($"Không tìm thấy phòng chat có Id {directChatId} .");
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Phòng chat không tồn tại",
                    data = null
                };
            }

            var messages = await _unitOfWork.GetRepository<Message>().GetListLimitAsync(
                selector: a => _mapper.Map<CreateMessageResponse>(a),
                predicate: a => a.IsActive && a.DirectChatId.Equals(directChatId) && (!beforeCursor.HasValue || a.CreateAt < beforeCursor.Value),
                orderBy: q => q.OrderByDescending(a => a.CreateAt),
                limit: 20);

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy tin nhắn thành công",
                data = messages
            };
        }

        public async Task<BaseResponse> GetMessageById(Guid messageId)
        {
            var message = await _unitOfWork.GetRepository<Message>().SingleOrDefaultAsync(
                predicate: m => m.Id.Equals(messageId) && m.IsActive);

            if (message == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tin nhắn với ID này",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy tin nhắn thành công",
                data = _mapper.Map<CreateMessageResponse>(message)
            };
        }

        public async Task<BaseResponse> RemoveMessage(Guid messageId)
        {
            var message = await _unitOfWork.GetRepository<Message>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(messageId) && x.IsActive);

            if (message == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tin nhắn với ID này",
                    data = null
                };
            }

            message.IsActive = false;
            message.DeleteAt = TimeUtil.GetCurrentSEATime();
            message.UpdateAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<Message>().UpdateAsync(message);

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Xóa tin nhắn thành công",
                    data = isSuccessfully
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Xóa tin nhắn thất bại",
                data = isSuccessfully
            };
        }

        public async Task<BaseResponse> SearchMessageByDirectChatId(Guid directChatId, string keyWord, int page, int size)
        {
            if (page < 1 || size < 1)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Page hoặc size không hợp lệ.",
                    data = null
                };
            }

            var messages = await _unitOfWork.GetRepository<Message>().GetPagingListAsync(
                selector: a => _mapper.Map<CreateMessageResponse>(a),
                predicate: a => a.IsActive && EF.Functions.Contains(a.Content, keyWord) && a.DirectChatId.Equals(directChatId),
                orderBy: q => q.OrderByDescending(a => a.CreateAt),
                page: page,
                size: size);

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy tin nhắn thành công",
                data = messages
            };
        }

        public async Task<BaseResponse> UpdateMessage(Guid messageId, UpdateMessageRequest request)
        {
            var message = await _unitOfWork.GetRepository<Message>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(messageId) && x.IsActive);

            if (message == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tin nhắn với ID này",
                    data = null
                };
            }

            message = _mapper.Map(request, message);

            _unitOfWork.GetRepository<Message>().UpdateAsync(message);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Cập nhật tin nhắn thành công",
                    data = _mapper.Map<CreateMessageResponse>(message)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Cập nhật tin nhắn thất bại",
                data = null
            };
        }
    }
}
