using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galini.Models.Payload.Response.Account;

namespace Galini.Models.Payload.Response.Transaction
{
    public class GetTransactionAdminResponse
    {
        public Guid Id { get; set; }
        public decimal? Amount { get; set; }
        public long? OrderCode { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public GetAccountResponse? GetAccountResponse { get; set; }
    }
}
