using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Request.Deposit
{
    public class CreateDepositRequest
    {
        public decimal Amount { get; set; }
        public string Description { get; set; } = null!;
    }
}
