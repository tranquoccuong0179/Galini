
using Galini.API.Constants;
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
        public async Task<IActionResult> CreateTopic([FromQuery] decimal balance)
        {
            var response = await _walletService.CreatePaymentUrlRegisterCreator(balance);
            return StatusCode(int.Parse(response.status), response);
        }
    }
}
