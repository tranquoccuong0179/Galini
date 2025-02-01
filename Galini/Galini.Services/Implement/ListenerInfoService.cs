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
using Galini.Models.Payload.Request.ListenerInfo;
using Galini.Models.Payload.Request.User;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.Account;
using Galini.Models.Payload.Response.ListenerInfo;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Galini.Utils;
using Microsoft.AspNetCore.Http;
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

        public Task<BaseResponse> GetAllListenerInfo(int page, int size)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> GetListenerInfoByAccountId(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> GetListenerInfoById(Guid listenerInfoId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> RemoveListenerInfo(Guid listenerInfoId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> UpdateListenerInfo(Guid listenerInfoId, CreateListenerInfoRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
