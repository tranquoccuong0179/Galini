using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Request.Bank
{
    public class BankRequest
    {
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
