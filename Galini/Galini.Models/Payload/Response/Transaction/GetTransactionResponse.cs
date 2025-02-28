using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Response.Transaction
{
    public class GetTransactionResponse
    {
        public decimal? Amount { get; set; }
        public long? OrderCode { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
    }
}
