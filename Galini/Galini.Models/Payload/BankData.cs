using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload
{
    public static class BankData
    {
        public static readonly Dictionary<string, string> BankCodes = new Dictionary<string, string>
        {
            { "Vietcombank", "970436" },
            { "BIDV", "970418" },
            { "Techcombank", "970407" },
            { "Agribank", "970405" },
            { "MB Bank", "970422" },
            { "ACB", "970416" },
            { "TPBank", "970423" },
            { "Sacombank", "970403" },
            { "VietinBank", "970415" },
            { "VPBank", "970432" },
            { "SHB", "970443" },
            { "HDBank", "970437" },
            { "Eximbank", "970431" },
            { "OCB", "970448" },
            { "SeABank", "970440" }
        };
    }
}
