
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

        /// <summary>
        /// API tạo liên kết thanh toán để nạp tiền vào ví.
        /// </summary>
        /// <remarks>
        /// - Nhận thông tin nạp tiền từ yêu cầu (`CreateDepositRequest`).  
        /// - Trả về liên kết thanh toán để người dùng thực hiện giao dịch.  
        /// - Nếu không tìm thấy người dùng hoặc ví của họ, trả về lỗi `404 Not Found`.  
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <param name="request">Dữ liệu yêu cầu nạp tiền.</param>
        /// <returns>
        /// - `200 OK`: Tạo liên kết thanh toán thành công.  
        /// - `404 Not Found`: Không tìm thấy người dùng hoặc ví của người dùng.  
        /// </returns>
        [HttpPost(ApiEndPointConstant.Wallet.CreateLink)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateLink([FromBody] CreateDepositRequest request)
        {
            var response = await _walletService.CreatePaymentUrlRegisterCreator(request);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpPost(ApiEndPointConstant.Wallet.Webhook)]
        public async Task<IActionResult> HandleWebhook([FromBody] WebhookType payload)
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

            try
            {
                var signatureFromPayOs = payload.signature;
                var requestBody = JsonConvert.SerializeObject(payload);
                var result = await _walletService.ConfirmWebhook(payload);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while handling webhook in controller.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the webhook.");
            }


        }

        /// <summary>
        /// API lấy thông tin số dư ví của người dùng.
        /// </summary>
        /// <remarks>
        /// - Trả về thông tin chi tiết về số dư hiện tại của ví.  
        /// - Nếu không tìm thấy người dùng hoặc ví của họ, trả về lỗi `404 Not Found`. 
        /// - Kết quả trả về được bọc trong `BaseResponse`.
        /// </remarks>
        /// <returns>
        /// - `200 OK`: Trả về thông tin ví thành công.  
        /// - `404 Not Found`: Không tìm thấy ví.
        /// </returns>
        [HttpGet(ApiEndPointConstant.Wallet.GetWallet)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetWallet()
        {
            var response = await _walletService.GetWallet();
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet("ReturnUrlFail")]
        public async Task<IActionResult> ReturnFailedUrl()
        {
            string responseCode = Request.Query["code"].ToString();
            string id = Request.Query["id"].ToString();
            string cancel = Request.Query["cancel"].ToString();
            string status = Request.Query["status"].ToString();
            string orderCode = Request.Query["orderCode"];

            if (status == "CANCELLED")
            {
                var response = await _walletService.HandleFailedPayment(id, long.Parse(orderCode));
                return Redirect("https://harmon-love.vercel.app/failed");
            }
            return Redirect("https://harmon-love.vercel.app/failed");
        }


    }
}
