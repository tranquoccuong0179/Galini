using Galini.Models.Payload.Request.Transaction;
using Galini.Models.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface ITransactionService
    {
        public Task<BaseResponse> GetAllTransaction(int page, int size, string? name, string? email, string? phone, bool? sortByPrice);
        public Task<BaseResponse> GetTransaction(Guid id);
        public Task<BaseResponse> GetTransactions(int page, int size);
    }
}
