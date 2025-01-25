using Galini.Models.Payload.Request.Review;
using Galini.Models.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface IReviewService
    {
        public Task<BaseResponse> CreateReview(CreateReviewRequest request, Guid bookingId);
        public Task<BaseResponse> GetAllReview(int page, int size);
        public Task<BaseResponse> GetReviewById(Guid reviewId);
        public Task<BaseResponse> UpdateReview(Guid reviewId, CreateReviewRequest request);
        public Task<BaseResponse> RemoveReview(Guid reviewId);
    }
}
