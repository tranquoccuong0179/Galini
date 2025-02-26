using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galini.Models.Payload.Response;
using Galini.Services.Interface;

namespace Galini.Services.Implement
{
    public class TransactionService : ITransactionService
    {
        public Task<BaseResponse> GetAllTransaction(int page, int size)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> GetTransaction(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> GetTransactions()
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> GetTransactionsForAdmin()
        {
            throw new NotImplementedException();
        }
    }
}
