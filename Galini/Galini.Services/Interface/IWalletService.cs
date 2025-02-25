using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galini.Models.Payload.Request.Deposit;
using Galini.Models.Payload.Response;

namespace Galini.Services.Interface
{
    public interface IWalletService
    {
        Task<BaseResponse> CreatePaymentUrlRegisterCreator(CreateDepositRequest request);
        Task<BaseResponse> ConfirmWebhook(string webhookUrl);
    }
}
