using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Azure;
using Galini.Models.Entity;
using Galini.Models.Enum;
using Galini.Models.Paginate;
using Galini.Models.Payload.Request.ListenerInfo;
using Galini.Models.Payload.Request.User;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.Account;
using Galini.Models.Payload.Response.ListenerInfo;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Galini.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Galini.Services.Implement
{
    public class ListenerInfoService : BaseService<ListenerInfoService>, IListenerInfoService
    {
        private readonly IMapper _mapper;
        public ListenerInfoService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<ListenerInfoService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _mapper = mapper;
        }

        public async Task<BaseResponse> CreateListenerInfo(RegisterUserRequest registerRequest, CreateListenerInfoRequest request)
        {
            if(registerRequest == null || request == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Dữ liệu đầu vào không hợp lệ",
                    data = null
                };
            }

            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(registerRequest.Email, emailPattern))
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Email không đúng định dạng",
                    data = null
                };
            }

            string phonePattern = @"^0\d{9}$";
            if (!Regex.IsMatch(registerRequest.Phone, phonePattern))
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Số điện thoại không đúng định dạng",
                    data = null
                };
            }

            var existingAccount = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: u => u.UserName == registerRequest.UserName || u.Email == registerRequest.Email || u.Phone == registerRequest.Phone);

            if (existingAccount != null)
            {
                string message = existingAccount.UserName == registerRequest.UserName ? "Username đã tồn tại" :
                                 existingAccount.Email == registerRequest.Email ? "Email đã tồn tại" :
                                 "Số điện thoại đã tồn tại";

                return new BaseResponse()
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = message,
                    data = null
                }; ;
            }

            var account = _mapper.Map<Account>(registerRequest);
            account.IsActive = true;
            account.Role = RoleEnum.Listener.GetDescriptionFromEnum();

            await _unitOfWork.GetRepository<Account>().InsertAsync(account);

            var listenerInfo = _mapper.Map<ListenerInfo>(request);
            listenerInfo.AccountId = account.Id;
            await _unitOfWork.GetRepository<ListenerInfo>().InsertAsync(listenerInfo);

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Thêm người nghe thành công",
                    data = _mapper.Map<CreateListenerInfoResponse>(listenerInfo)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Thêm người nghe thất bại",
                data = null
            };
        }

        public async Task<BaseResponse> GetAllListenerInfo(int page, int size)
        {
            var listenerInfo = await _unitOfWork.GetRepository<ListenerInfo>().GetPagingListAsync(
                selector: l => _mapper.Map<GetListenerInfoResponse>(l),
                orderBy: l => l.OrderByDescending(l => l.CreateAt),
                predicate: l => l.IsActive == true,
                include: l => l.Include(l => l.Account),
                page: page,
                size: size);

            int totalItems = listenerInfo.Total;
            int totalPages = (int)Math.Ceiling((double)totalItems / size);

            if (listenerInfo == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Danh sách người nghe",
                    data = new Paginate<ListenerInfo>()
                    {
                        Page = page,
                        Size = size,
                        Total = totalItems,
                        TotalPages = totalPages,
                        Items = new List<ListenerInfo>()
                    }
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Danh sách người nghe",
                data = listenerInfo
            };
        }

        public async Task<BaseResponse> GetListenerInfoByAccountId(Guid accountId)
        {
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: l => l.Id.Equals(accountId) && l.IsActive);

            if (account == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy người nghe",
                    data = null
                };
            }

            var listenerInfo = await _unitOfWork.GetRepository<ListenerInfo>().SingleOrDefaultAsync(
                predicate: l => l.AccountId.Equals(accountId) && l.IsActive,
                include: l => l.Include(l => l.Account));

            if (listenerInfo == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy người nghe",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Thông tin người nghe",
                data = _mapper.Map<GetListenerInfoResponse>(listenerInfo)
            };
        }

        public async Task<BaseResponse> GetListenerInfoById(Guid id)
        {
            var listenerInfo = await _unitOfWork.GetRepository<ListenerInfo>().SingleOrDefaultAsync(
                predicate: l => l.Id.Equals(id) && l.IsActive == true,
                include: l => l.Include(l => l.Account));

            if(listenerInfo == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy người nghe",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Thông tin người nghe",
                data = _mapper.Map<GetListenerInfoResponse>(listenerInfo)
            };
        }

        public async Task<BaseResponse> RemoveListenerInfo(Guid id)
        {
            var listenerInfo = await _unitOfWork.GetRepository<ListenerInfo>().SingleOrDefaultAsync(
                predicate: l => l.Id.Equals(id) && l.IsActive);

            if (listenerInfo == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy người nghe",
                    data = false
                };
            }

            var topics = await _unitOfWork.GetRepository<Topic>().GetListAsync(
                predicate: t => t.ListenerInfoId.Equals(listenerInfo.Id) && t.IsActive);

            if (topics != null)
            {
                foreach (var topic in topics)
                {
                    topic.IsActive = false;
                    topic.UpdateAt = TimeUtil.GetCurrentSEATime();
                    topic.DeleteAt = TimeUtil.GetCurrentSEATime();
                    _unitOfWork.GetRepository<Topic>().UpdateAsync(topic);
                }
            }

            listenerInfo.IsActive = false;
            listenerInfo.DeleteAt = TimeUtil.GetCurrentSEATime();
            listenerInfo.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<ListenerInfo>().UpdateAsync(listenerInfo);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Xóa người nghe thành công",
                    data = true
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Xóa người nghe thất bại",
                data = false
            };
        }

        public async Task<BaseResponse> UpdateListenerInfo(Guid id, UpdateListenerInfoRequest request)
        {
            var listenerInfo = await _unitOfWork.GetRepository<ListenerInfo>().SingleOrDefaultAsync(
                predicate: l => l.Id.Equals(id) && l.IsActive == true,
                include: l => l.Include(l => l.Account));

            if (listenerInfo == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy người nghe",
                    data = null
                };
            }

            _mapper.Map(request, listenerInfo);
            _unitOfWork.GetRepository<ListenerInfo>().UpdateAsync(listenerInfo);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Cập nhật người nghe thành công",
                    data = _mapper.Map<GetListenerInfoResponse>(listenerInfo)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Cập nhật người nghe thất bại",
                data = null
            };
        }
    }
}
