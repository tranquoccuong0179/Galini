using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galini.Models.Payload.Request.Bank;

namespace Galini.Services.Interface
{
    public interface IBankService
    {
        string GetBankCode(string bankName);
        Dictionary<string, string> GetAllBanks();
        bool IsValidBank(string bankName);
        string GeneratePaymentQRCode(BankRequest request);
    }
}
