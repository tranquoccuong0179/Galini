using Galini.API.Constants;
using Galini.Models.Enum;
using Galini.Models.Payload.Request.CallHistory;
using Galini.Models.Payload.Request.FriendShip;
using Galini.Models.Payload.Request.Review;
using Galini.Models.Payload.Response;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    public class ReviewController : BaseController<ReviewController>
    {
        private readonly IReviewService _reviewService;
        public ReviewController(ILogger<ReviewController> logger, IReviewService reviewService) : base(logger)
        {
            _reviewService = reviewService;
        }

        [HttpPost(ApiEndPointConstant.Review.CreateReview)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequest request, [FromQuery] Guid bookingId)
        {
            if (request == null)
            {
                return BadRequest(new BaseResponse
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Invalid request data",
                    data = null
                });
            }

            var response = await _reviewService.CreateReview(request, bookingId);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.Review.GetAllReview)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllReview([FromQuery] int? page, [FromQuery] int? size, [FromQuery] int? star,
                                                                                                    [FromQuery] bool? sortByStar,
                                                                                                    [FromQuery] Guid? id)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _reviewService.GetAllReview(pageNumber, pageSize, star, sortByStar, id);

            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.Review.GetAllReviewByListenerId)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllReviewByListenerId([FromRoute] Guid id, [FromQuery] int? star, [FromQuery] bool? sortByStar, [FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _reviewService.GetAllReviewByListenerId(pageNumber, pageSize, star, sortByStar, id);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpGet(ApiEndPointConstant.Review.GetReviewById)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetReviewById([FromRoute] Guid id)
        {
            var response = await _reviewService.GetReviewById(id);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpPatch(ApiEndPointConstant.Review.UpdateReview)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateReview([FromRoute] Guid id, [FromBody] UpdateReviewRequest request)
        {

            var response = await _reviewService.UpdateReview(id, request);
            return StatusCode(int.Parse(response.status), response);
        }

        [HttpDelete(ApiEndPointConstant.Review.RemoveReview)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RemoveReview([FromRoute] Guid id)
        {
            var response = await _reviewService.RemoveReview(id);

            return StatusCode(int.Parse(response.status), response);
        }
    }    
}
