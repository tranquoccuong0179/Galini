using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Payload.Request.Review;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.Review;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Galini.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Implement
{
    public class ReviewService : BaseService<ReviewService>, IReviewService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public ReviewService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<ReviewService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor; 
        }

        public async Task<BaseResponse> CreateReview(CreateReviewRequest request, Guid id)
        {
            var bookingExist = await _unitOfWork.GetRepository<Booking>().SingleOrDefaultAsync(
                predicate: l => l.Id.Equals(id) && l.IsActive);

            if (bookingExist == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy booking",
                    data = null
                };
            }

            var listener = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: l => l.Id.Equals(bookingExist.ListenerId) && l.IsActive);

            if (listener == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy người nghe",
                    data = null
                };
            }

            var review = _mapper.Map<CreateReviewRequest, Review>(request);
            review.BookingId = id;
            review.ListenerId = listener.Id;

            var data = _mapper.Map<CreateReviewResponse>(review);
            data.ListenerName = listener.FullName;

            await _unitOfWork.GetRepository<Review>().InsertAsync(review);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Thêm đánh giá thành công",
                    data = data
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Thêm đánh giá thất bại",
                data = null
            };
        }

        public async Task<BaseResponse> GetAllReview(int page, int size, int? star, bool? sortByStar, Guid? id)
        {
            if (page < 1 || size < 1)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Page hoặc size không hợp lệ.",
                    data = null
                };
            }

            var review = await _unitOfWork.GetRepository<Review>().GetPagingListAsync(
                selector: a => _mapper.Map<CreateReviewResponse>(a),
                predicate: a => a.IsActive && (!star.HasValue || a.Star >= star) && (!id.HasValue || a.BookingId.Equals(id)),
                orderBy: l => sortByStar.HasValue ? (sortByStar.Value ? l.OrderBy(l => l.Star) : l.OrderByDescending(l => l.Star)) : l.OrderBy(l => l.CreateAt),
                page: page,
                size: size);

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy đánh giá thành công",
                data = review
            };
        }

        public async Task<BaseResponse> GetAllReviewByListenerId(int page, int size, int? star, bool? sortByStar, Guid id)
        {
            if (page < 1 || size < 1)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Page hoặc size không hợp lệ.",
                    data = null
                };
            }

            var review = await _unitOfWork.GetRepository<Review>().GetPagingListAsync(
                selector: a => _mapper.Map<CreateReviewResponse>(a),
                predicate: a => a.IsActive && (!star.HasValue || a.Star >= star) && a.ListenerId.Equals(id),
                orderBy: l => sortByStar.HasValue ? (sortByStar.Value ? l.OrderBy(l => l.Star) : l.OrderByDescending(l => l.Star)) : l.OrderBy(l => l.CreateAt),
                page: page,
                size: size);

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy đánh giá thành công",
                data = review
            };
        }

        public async Task<BaseResponse> GetReviewById(Guid id)
        {          
            var review = await _unitOfWork.GetRepository<Review>().SingleOrDefaultAsync(
                selector: t => _mapper.Map<CreateReviewResponse>(t),
                predicate: t => t.Id.Equals(id) && t.IsActive);

            if (review == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy đánh giá này",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy đánh giá thành công",
                data = review
            };
        }

        public async Task<BaseResponse> RemoveReview(Guid id)
        {
            var review = await _unitOfWork.GetRepository<Review>().SingleOrDefaultAsync(
                predicate: t => t.Id.Equals(id) && t.IsActive);

            if (review == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy đánh giá này",
                    data = null
                };
            }

            review.IsActive = false;
            review.DeleteAt = TimeUtil.GetCurrentSEATime();
            review.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<Review>().UpdateAsync(review);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Xóa đánh giá thành công",
                    data = true
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Xóa đánh giá thất bại",
                data = false
            };
        }

        public async Task<BaseResponse> UpdateReview(Guid id, UpdateReviewRequest request)
        {
            var review = await _unitOfWork.GetRepository<Review>().SingleOrDefaultAsync(
                predicate: t => t.Id.Equals(id) && t.IsActive);

            if (review == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy đánh giá này",
                    data = null
                };
            }

            _mapper.Map(request, review);
            _unitOfWork.GetRepository<Review>().UpdateAsync(review);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Cập nhật đánh giá thành công",
                    data = _mapper.Map<CreateReviewResponse>(review)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Cập nhật đánh giá thất bại",
                data = null
            };
        }
    }
}
