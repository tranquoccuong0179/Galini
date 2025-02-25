
using Galini.API.Constants;
using Galini.Models.Payload.Request.Deposit;
using Galini.Models.Payload.Request.Topic;
using Galini.Models.Payload.Response;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Newtonsoft.Json;

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

        [HttpPost("api/v1/webhook-url")]// Đặt route cụ thể cho action method
        public IActionResult HandleWebhook(WebhookType payload)
        {
            //try
            //{
            //    // Đọc body của request
            //    using (var reader = new StreamReader(Request.Body))
            //    {
            //        var jsonBody = reader.ReadToEnd();
            //        var data = JsonConvert.DeserializeObject<dynamic>(jsonBody);

            //        // Xử lý dữ liệu webhook ở đây
            //        // Ví dụ: lấy orderCode từ dữ liệu webhook
            //        var orderCode = data?["orderCode"]?.ToString();
            //        Console.WriteLine($"Received webhook for order: {orderCode}");

            //        // Trả về response thành công
            //        return Ok(new
            //        {
            //            code = "00",
            //            message = "success"
            //        });
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // Log lỗi (nếu cần)
            //    Console.WriteLine($"Error processing webhook: {ex.Message}");

            //    // Trả về response với status code 200 ngay cả khi có lỗi
            //    return Ok(new
            //    {
            //        code = "00",
            //        message = "success"
            //    });
            //}

            var response = _walletService.ConfirmWebhook(payload);


            return Ok(response);

        }


    }
}
