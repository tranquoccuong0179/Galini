using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Paginate;
using Galini.Models.Payload.Request.DirectChat;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.DirectChat;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Galini.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Galini.Services.Implement
{
    public class DirectChatService : BaseService<DirectChatService>, IDirectChatService
    {
        public DirectChatService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<DirectChatService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse> CreateDirectChat(CreateDirectChatRequest request)
        {
            var directChat = _mapper.Map<DirectChat>(request);
            await _unitOfWork.GetRepository<DirectChat>().InsertAsync(directChat);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;
            if (isSuccessfully)
            {
                return new BaseResponse
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Tạo thành công",
                    data = _mapper.Map<CreateDirectChatResponse>(directChat)
                };
            }

            return new BaseResponse
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Tạo thất bại",
                data = null
            };
        }

        public async Task<BaseResponse> GetAllDirectChat(int page, int size, string? name)
        {
            var response = await _unitOfWork.GetRepository<DirectChat>().GetPagingListAsync(
                selector: d => _mapper.Map<GetDirectChatResponse>(d),
                predicate: d => d.IsActive && (string.IsNullOrEmpty(name) || d.Name.Contains(name)),
                orderBy: d => d.OrderByDescending(d => d.UpdateAt),
                page: page,
                size: size);
            int totalItems = response.Total;
            int totalPages = (int)Math.Ceiling((double)totalItems / size);

            if (response == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Danh sách direct chat",
                    data = new Paginate<DirectChat>()
                    {
                        Page = page,
                        Size = size,
                        Total = totalItems,
                        TotalPages = totalPages,
                        Items = new List<DirectChat>()
                    }
                };
            }

            return new BaseResponse
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Danh sách direct chat",
                data = response
            };
        }

        public async Task<BaseResponse> GetDirectChatById(Guid id)
        {
            var response = await _unitOfWork.GetRepository<DirectChat>().SingleOrDefaultAsync(
                selector: d => _mapper.Map<GetDirectChatResponse>(d),
                predicate: d => d.IsActive && d.Id.Equals(id));

            if (response == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Direct chat không tồn tại",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Direct chat",
                data = response
            };
        }

        public async Task<BaseResponse> RemoveDirectChat(Guid id)
        {
            var directChat = await _unitOfWork.GetRepository<DirectChat>().SingleOrDefaultAsync(
                predicate: d => d.IsActive && d.Id.Equals(id));

            if (directChat == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Direct chat không tồn tại",
                    data = null
                };
            }

            directChat.IsActive = false;
            directChat.UpdateAt = TimeUtil.GetCurrentSEATime();
            directChat.DeleteAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<DirectChat>().UpdateAsync(directChat);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Xóa direct chat thành công",
                    data = true
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Xóa direct chat thất bại",
                data = false
            };
        }

        public async Task<BaseResponse> UpdateDirectChat(Guid id, UpdateDirectChatRequest request)
        {
            var directChat = await _unitOfWork.GetRepository<DirectChat>().SingleOrDefaultAsync(
                predicate: d => d.IsActive && d.Id.Equals(id));

            if (directChat == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Direct chat không tồn tại",
                    data = null
                };
            }

            directChat.Name = string.IsNullOrEmpty(request.Name) ? directChat.Name : request.Name;
            directChat.UpdateAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<DirectChat>().UpdateAsync(directChat);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Cập nhật direct chat thành công",
                    data = _mapper.Map<GetDirectChatResponse>(directChat)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Cập nhật direct chat thất bại",
                data = null
            };
        }
    }
}
