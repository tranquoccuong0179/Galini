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
        public Task<BaseResponse> CreateTransaction(CreateTransactionRequest request, Guid walletId);
        public Task<BaseResponse> GetAllTransaction(int page, int size);
        public Task<BaseResponse> GetTransactionById(Guid transactionId);
        public Task<BaseResponse> GetTransactionByAccountId(Guid walletId);
        public Task<BaseResponse> UpdateTransaction(Guid transactionId, CreateTransactionRequest request);
        public Task<BaseResponse> RemoveTransaction(Guid transactionId);
    }
}
