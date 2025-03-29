
using Galini.Models.Payload.Request.Bank;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    [ApiController]
    [Route("api/bank")]
    public class BankController : BaseController<BankController>
    {
        private readonly IBankService _bankService;
        public BankController(ILogger<BankController> logger, IBankService bankService) : base(logger)
        {
            _bankService = bankService;
        }

        [HttpPost("generate-qr")]
        public async Task<IActionResult> GenerateQRCode([FromBody] BankRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Dữ liệu không hợp lệ.");
                }

                string qrFilePath = _bankService.GeneratePaymentQRCode(request);
                return Ok(new { message = "Tạo QR thành công", qrPath = qrFilePath });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo QR code");
                return StatusCode(500, "Đã xảy ra lỗi nội bộ.");
            }
        }
    }
}
