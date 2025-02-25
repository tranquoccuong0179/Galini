
using Galini.API.Constants;
using Galini.Models.Payload.Request.Deposit;
using Galini.Models.Payload.Request.Topic;
using Galini.Models.Payload.Response;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class WalletController : BaseController<WalletController>
    {
        private readonly IWalletService _walletService;
        public WalletController(ILogger<WalletController> logger, IWalletService walletService) : base(logger)
        {
            _walletService = walletService;
        }

        [HttpPost(ApiEndPointConstant.Wallet.CreateLink)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateLink([FromBody] CreateDepositRequest request)
        {
            var response = await _walletService.CreatePaymentUrlRegisterCreator(request);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpPost("confirm-webhook")]
        public async Task<IActionResult> ConfirmWebhook()
        {
            var webhookLink = "https://yourapi/api/v1/wallet/webhook";
            var result = await _walletService.ConfirmWebhook(webhookLink);
            return Ok(result);
        }
    }
}
