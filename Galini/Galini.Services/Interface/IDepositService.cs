using Galini.Models.Request.Deposit;
using Galini.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface IDepositService
    {
        public Task<BaseResponse> CreateDeposit(CreateDepositRequest request, Guid accountId);
        public Task<BaseResponse> GetAllDeposit(int page, int size);
        public Task<BaseResponse> GetDepositById(Guid depositId);
        public Task<BaseResponse> GetDepositByAccountId(Guid accountId);
        public Task<BaseResponse> UpdateDeposit(Guid depositId, CreateDepositRequest request);
        public Task<BaseResponse> RemoveDeposit(Guid depositId);
    }
}
