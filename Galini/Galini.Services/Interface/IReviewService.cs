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
        public Task<BaseResponse> CreateReview(CreateReviewRequest request, Guid id);
        public Task<BaseResponse> GetAllReview(int page, int size, int? star, bool? sortByStar, Guid? id);
        Task<BaseResponse> GetAllReviewByListenerId(int page, int size, int? star, bool? sortByStar, Guid id);
        public Task<BaseResponse> GetReviewById(Guid id);
        public Task<BaseResponse> UpdateReview(Guid id, UpdateReviewRequest request);
        public Task<BaseResponse> RemoveReview(Guid id);
    }
}
