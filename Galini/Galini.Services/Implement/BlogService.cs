using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Payload.Request.Blog;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.Topic;
using Galini.Models.Utils;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Galini.Services.Implement
{
    public class BlogService : BaseService<BlogService>, IBlogService
    {
        private readonly HtmlSanitizerUtil _sanitizer;

        public BlogService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<BlogService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, HtmlSanitizerUtil sanitizer) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _sanitizer = sanitizer;
        }

        public async Task<BaseResponse> CreateBlog(CreateBlogRequest request)
        {
            Guid? id = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.IsActive && a.Id.Equals(id));

            if (account == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Người dùng không tồn tại",
                    data = null
                };
            }
            request.Content = _sanitizer.Sanitize(request.Content);
            var blog = _mapper.Map<Blog>(request);
            blog.AuthorId = account.Id;
            blog.Content = request.Content;

            await _unitOfWork.GetRepository<Blog>().InsertAsync(blog);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Thêm blog thành công",
                    data = _mapper.Map<CreateTopicResponse>(blog)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Thêm blog thất bại",
                data = null
            };
        }

        public Task<BaseResponse> GetAllBlogs(int page, int size)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> GetBlogById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> RemoveBlog(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> UpdateBlog(Guid id, UpdateBlogRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
