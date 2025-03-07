using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Paginate;
using Galini.Models.Payload.Request.DirectChatParticipant;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.DirectChatParticipant;
using Galini.Models.Payload.Response.Topic;
using Galini.Models.Utils;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Galini.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Galini.Services.Implement
{
    public class DirectChatParticipantService : BaseService<DirectChatParticipantService>, IDirectChatParticipantService
    {
        public DirectChatParticipantService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<DirectChatParticipantService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse> CreateDirectChatParticipant(CreateDirectChatParticipant request)
        {
            Guid? id = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(id) && a.IsActive == true);

            if (account == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tài khoản",
                    data = null
                };
            }

            var directChat = await _unitOfWork.GetRepository<DirectChat>().SingleOrDefaultAsync(
                predicate: d => d.Id.Equals(request.DirectChatId) && d.IsActive);

            if (directChat == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy đoạn chat",
                    data = null
                };
            }

            var response = _mapper.Map<DirectChatParticipant>(request);
            response.AccountId = account.Id;

            await _unitOfWork.GetRepository<DirectChatParticipant>().InsertAsync(response);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Thêm thành công",
                    data = _mapper.Map<CreateDirectChatParticipantResponse>(response)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Thêm thất bại",
                data = null
            };

        }

        public async Task<BaseResponse> GetAllDirectChatParticipant(int page, int size)
        {

            Guid? id = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(id) && a.IsActive == true);

            if (account == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tài khoản",
                    data = null
                };
            }
            var response = await _unitOfWork.GetRepository<DirectChatParticipant>().GetPagingListAsync(
                selector: d => _mapper.Map<GetDirectChatParticipantResponse>(d),
                predicate: d => d.IsActive && d.AccountId.Equals(account.Id),
                page: page,
                size: size);

            int totalItems = response.Total;
            int totalPages = (int)Math.Ceiling((double)totalItems / size);

            if (response == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Danh sách",
                    data = new Paginate<DirectChatParticipant>()
                    {
                        Page = page,
                        Size = size,
                        Total = totalItems,
                        TotalPages = totalPages,
                        Items = new List<DirectChatParticipant>()
                    }
                };
            }

            return new BaseResponse
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Danh sách",
                data = response
            };
        }

        public async Task<BaseResponse> GetDirectChatParticipantById(Guid id)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true);

            if (account == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tài khoản",
                    data = null
                };
            }
            var response = await _unitOfWork.GetRepository<DirectChatParticipant>().SingleOrDefaultAsync(
                selector: d => _mapper.Map<GetDirectChatParticipantResponse>(d),
                predicate: d => d.Id.Equals(id) && d.IsActive && d.AccountId.Equals(account.Id));

            if(response == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Tìm thấy",
                data = response
            };
        }

        public async Task<BaseResponse> RemoveDirectChatParticipant(Guid id)
        {
            var direct = await _unitOfWork.GetRepository<DirectChatParticipant>().SingleOrDefaultAsync(
                predicate: d => d.Id.Equals(id) && d.IsActive);
            if(direct == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy",
                    data = false
                };
            }

            direct.IsActive = false;
            direct.UpdateAt = TimeUtil.GetCurrentSEATime();
            direct.DeleteAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<DirectChatParticipant>().UpdateAsync(direct);

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Xóa thành công",
                    data = true
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Xóa thất bại",
                data = false
            };
        }

        public async Task<BaseResponse> UpdateDirectChatParticipant(Guid id, UpdateDirectChatParticipant request)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(id) && a.IsActive == true);

            if (account == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tài khoản",
                    data = null
                };
            }
            var directChatParticipant = await _unitOfWork.GetRepository<DirectChatParticipant>().SingleOrDefaultAsync(
                predicate: d => d.Id.Equals(id) && d.IsActive && d.AccountId.Equals(account.Id));
            if (directChatParticipant == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy",
                    data = null
                };
            }

            directChatParticipant.NickName = string.IsNullOrEmpty(request.NickName) ? directChatParticipant.NickName : request.NickName;
            _unitOfWork.GetRepository<DirectChatParticipant>().UpdateAsync(directChatParticipant);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Cập nhật thành công",
                    data = _mapper.Map<GetDirectChatParticipantResponse>(directChatParticipant)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Cập nhật thất bại",
                data = null
            };
        }
    }
}
