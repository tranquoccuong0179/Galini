using Galini.Models.Payload.Request.Question;
using Galini.Models.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface IQuestionService
    {
        public Task<BaseResponse> CreateQuestion(CreateQuestionRequest request);
        public Task<BaseResponse> GetAllQuestion(int page, int size);
        public Task<BaseResponse> GetQuestionById(Guid id);
        public Task<BaseResponse> UpdateQuestion(Guid id, CreateQuestionRequest request);
        public Task<BaseResponse> RemoveQuestion(Guid id);
    }
}
