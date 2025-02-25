using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Paginate;
using Galini.Models.Payload.Request.Question;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.Notification;
using Galini.Models.Payload.Response.Question;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Galini.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Galini.Services.Implement
{
    public class QuestionService : BaseService<QuestionService>, IQuestionService
    {
        public QuestionService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<QuestionService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse> CreateQuestion(CreateQuestionRequest request)
        {
            var questionExist = await _unitOfWork.GetRepository<Question>().SingleOrDefaultAsync(
                predicate: q => q.Content.Equals(request.Content));
            if(questionExist != null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Nội dung câu hỏi đã tồn tại",
                    data = null
                };
            }

            var question = _mapper.Map<Question>(request);
            await _unitOfWork.GetRepository<Question>().InsertAsync(question);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;
            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Tạo câu hỏi thành công",
                    data = _mapper.Map<CreateQuestionResponse>(question)
                };
            }
            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Tạo câu hỏi thất bại",
                data = null
            };
        }

        public async Task<BaseResponse> GetAllQuestion(int page, int size)
        {
            var response = await _unitOfWork.GetRepository<Question>().GetPagingListAsync(
                selector: q => _mapper.Map<GetQuestionResponse>(q),
                orderBy: q => q.OrderByDescending(q => q.CreateAt),
                predicate: q => q.IsActive == true,
                page: page,
                size: size);

            int totalItems = response.Total;
            int totalPages = (int)Math.Ceiling((double)totalItems / size);

            if (response == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Danh sách câu hỏi",
                    data = new Paginate<Question>()
                    {
                        Page = page,
                        Size = size,
                        Total = totalItems,
                        TotalPages = totalPages,
                        Items = new List<Question>()
                    }
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Danh sách câu hỏi",
                data = response
            };
        }

        public async Task<BaseResponse> GetQuestionById(Guid id)
        {
            var response = await _unitOfWork.GetRepository<Question>().SingleOrDefaultAsync(
                selector: q => _mapper.Map<GetQuestionResponse>(q),
                predicate: q => q.IsActive == true && q.Id.Equals(id));
            if (response == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy câu hỏi",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "câu hỏi",
                data = response
            };
        }

        public async Task<BaseResponse> RemoveQuestion(Guid id)
        {
            var question = await _unitOfWork.GetRepository<Question>().SingleOrDefaultAsync(
                predicate: q => q.IsActive == true && q.Id.Equals(id));

            if (question == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy câu hỏi",
                    data = false
                };
            }

            question.IsActive = false;
            question.DeleteAt = TimeUtil.GetCurrentSEATime();
            question.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<Question>().UpdateAsync(question);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Xóa câu hỏi thành công",
                    data = true
                };
            }
            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Xóa câu hỏi thất bại",
                data = false
            };
        }

        public async Task<BaseResponse> UpdateQuestion(Guid id, UpdateQuestionRequest request)
        {
            var question = await _unitOfWork.GetRepository<Question>().SingleOrDefaultAsync(
                predicate: q => q.Id.Equals(id) && q.IsActive == true);
            if (question == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy câu hỏi",
                    data = null
                };
            }

            _mapper.Map(request, question);
            _unitOfWork.GetRepository<Question>().UpdateAsync(question);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Cập nhật câu hỏi thành công",
                    data = _mapper.Map<GetQuestionResponse>(question)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Cập nhật câu hỏi thất bại",
                data = null
            };
        }
    }
}
