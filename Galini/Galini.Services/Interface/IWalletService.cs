using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galini.Models.Payload.Response;

namespace Galini.Services.Interface
{
    public interface IWalletService
    {
        Task<BaseResponse> CreatePaymentUrlRegisterCreator(decimal balance);
        Task<BaseResponse> ConfirmWebhook(string webhookUrl);
    }
}
