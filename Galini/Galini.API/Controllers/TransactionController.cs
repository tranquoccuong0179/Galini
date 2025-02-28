
using Galini.API.Constants;
using Galini.Models.Enum;
using Galini.Models.Payload.Response;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class TransactionController : BaseController<TransactionController>
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ILogger<TransactionController> logger, ITransactionService transactionService) : base(logger)
        {
            _transactionService = transactionService;
        }

        [HttpGet(ApiEndPointConstant.Transaction.GetTransactions)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetTransactions([FromQuery] int? page, 
                                                         [FromQuery] int? size, 
                                                         [FromQuery] int? daysAgo, 
                                                         [FromQuery] int? weeksAgo, 
                                                         [FromQuery] int? monthsAgo)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _transactionService.GetTransactions(pageNumber, pageSize, daysAgo, weeksAgo, monthsAgo);

            return StatusCode(int.Parse(response.status), response);
        }
        
        [HttpGet(ApiEndPointConstant.Transaction.GetTransactionsForAdmin)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetTransactionsForAdmin([FromQuery] int? page, 
                                                                 [FromQuery] int? size,
                                                                 [FromQuery] string? name,
                                                                 [FromQuery] string? email,
                                                                 [FromQuery] string? phone,
                                                                 [FromQuery] TransactionStatusEnum? status,
                                                                 [FromQuery] TransactionTypeEnum? type,
                                                                 [FromQuery] bool? sortByPrice,
                                                                 [FromQuery] int? daysAgo,
                                                                 [FromQuery] int? weeksAgo,
                                                                 [FromQuery] int? monthsAgo)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _transactionService.GetAllTransaction(pageNumber, pageSize, name, email, phone, status, type, sortByPrice, daysAgo, weeksAgo, monthsAgo);

            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.Transaction.GetTransactionById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetTransactionById([FromRoute] Guid id)
        {
            var response = await _transactionService.GetTransaction(id);

            return StatusCode(int.Parse(response.status), response);
        }
    }
}
