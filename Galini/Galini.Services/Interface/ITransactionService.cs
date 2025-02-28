using Galini.Models.Enum;
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
        public Task<BaseResponse> GetAllTransaction(int page, int size, string? name, string? email, string? phone, TransactionStatusEnum? status, TransactionTypeEnum? type, bool? sortByPrice, int? daysAgo, int? weeksAgo, int? monthsAgo);
        public Task<BaseResponse> GetTransaction(Guid id);
        public Task<BaseResponse> GetTransactions(int page, int size, int? daysAgo, int? weeksAgo, int? monthsAgo);
    }
}
