using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Paginate;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.Transaction;
using Galini.Models.Utils;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Galini.Services.Implement
{
    public class TransactionService : BaseService<TransactionService>, ITransactionService
    {
        public TransactionService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<TransactionService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse> GetAllTransaction(int page, 
                                                          int size, 
                                                          string? name, 
                                                          string? email, 
                                                          string? phone, 
                                                          bool? sortByPrice)
        {
            var response = await _unitOfWork.GetRepository<Transaction>().GetPagingListAsync(
                selector: t => _mapper.Map<GetTransactionAdminResponse>(t),
                predicate: t => (string.IsNullOrEmpty(name) || t.Wallet.Account.UserName.Contains(name)) &&
                                (string.IsNullOrEmpty(email) || t.Wallet.Account.Email.Equals(email)) &&
                                (string.IsNullOrEmpty(phone) || t.Wallet.Account.Phone.Equals(phone)),
                include: t => t.Include(t => t.Wallet)
                               .ThenInclude(w => w.Account),
                orderBy: t => sortByPrice.HasValue ?
                        (sortByPrice.Value ? t.OrderBy(t => t.Amount) : t.OrderByDescending(t => t.Amount)) :
                        t.OrderByDescending(t => t.CreateAt),
                page: page,
                size: size);

            int totalItems = response.Total;
            int totalPages = (int)Math.Ceiling((double)totalItems / size);

            if (response == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Danh sách giao dịch",
                    data = new Paginate<Transaction>()
                    {
                        Page = page,
                        Size = size,
                        Total = totalItems,
                        TotalPages = totalPages,
                        Items = new List<Transaction>()
                    }
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Danh sách chủ đề",
                data = response
            };
        }

        public async Task<BaseResponse> GetTransaction(Guid id)
        {
            Guid? userId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var user = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: u => u.Id.Equals(userId) && u.IsActive == true);
            if (user == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Người dùng không tồn tại",
                    data = null
                };
            }

            var response = await _unitOfWork.GetRepository<Transaction>().SingleOrDefaultAsync(
                selector: t => _mapper.Map<GetTransactionResponse>(t),
                predicate: t => t.Id.Equals(id) && t.Wallet.AccountId.Equals(user.Id));
            if (response == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Giao dịch không tồn tại",
                    data = null
                };
            }


            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Tìm thấy giao dịch",
                data = response
            };
        }

        public async Task<BaseResponse> GetTransactions(int page, int size)
        {
            Guid? userId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var user = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: u => u.Id.Equals(userId) && u.IsActive == true);
            if (user == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Người dùng không tồn tại",
                    data = null
                };
            }

            var response = await _unitOfWork.GetRepository<Transaction>().GetPagingListAsync(
                selector: t => _mapper.Map<GetTransactionResponse>(t),
                predicate: t => t.Wallet.AccountId.Equals(user.Id),
                page: page,
                size: size);

            int totalItems = response.Total;
            int totalPages = (int)Math.Ceiling((double)totalItems / size);

            if (response == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Danh sách giao dịch",
                    data = new Paginate<Transaction>()
                    {
                        Page = page,
                        Size = size,
                        Total = totalItems,
                        TotalPages = totalPages,
                        Items = new List<Transaction>()
                    }
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Danh sách chủ đề",
                data = response
            };
        }
    }
}
