using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Paginate;
using Galini.Models.Payload.Request.Blog;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.Blog;
using Galini.Models.Payload.Response.Topic;
using Galini.Models.Utils;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Galini.Utils;
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

        public async Task<BaseResponse> GetAllBlogs(int page, int size)
        {
            var response = await _unitOfWork.GetRepository<Blog>().GetPagingListAsync(
                selector: b => _mapper.Map<GetBlogResponse>(b),
                predicate: b => b.IsActive,
                page: page,
                size: size);

            int totalItems = response.Total;
            int totalPages = (int)Math.Ceiling((double)totalItems / size);

            if (response == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Danh sách blog",
                    data = new Paginate<Blog>()
                    {
                        Page = page,
                        Size = size,
                        Total = totalItems,
                        TotalPages = totalPages,
                        Items = new List<Blog>()
                    }
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Danh sách blog",
                data = response
            };
        }

        public async Task<BaseResponse> GetBlogById(Guid id)
        {
            var response = await _unitOfWork.GetRepository<Blog>().SingleOrDefaultAsync(
                selector: b => _mapper.Map<GetBlogResponse>(b),
                predicate: b => b.IsActive && b.Id.Equals(id));

            if(response == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy blog",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Tìm thấy blog",
                data = response
            };
        }

        public async Task<BaseResponse> RemoveBlog(Guid id)
        {
            Guid? userId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.IsActive && a.Id.Equals(userId));

            if (account == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Người dùng không tồn tại",
                    data = null
                };
            }

            var blog = await _unitOfWork.GetRepository<Blog>().SingleOrDefaultAsync(
                predicate: b => b.IsActive && b.AuthorId.Equals(userId) && b.Id.Equals(id));

            if (blog == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy blog",
                    data = null
                };
            }


            blog.DeleteAt = TimeUtil.GetCurrentSEATime();
            blog.IsActive = false;
            blog.UpdateAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<Blog>().UpdateAsync(blog);

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Xóa blog thành công",
                    data = _mapper.Map<CreateTopicResponse>(blog)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Xóa blog thất bại",
                data = null
            };
        }

        public async Task<BaseResponse> UpdateBlog(Guid id, UpdateBlogRequest request)
        {
            Guid? userId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.IsActive && a.Id.Equals(userId));

            if (account == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Người dùng không tồn tại",
                    data = null
                };
            }

            var blog = await _unitOfWork.GetRepository<Blog>().SingleOrDefaultAsync(
                predicate: b => b.IsActive && b.AuthorId.Equals(userId) && b.Id.Equals(id));

            if (blog == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy blog",
                    data = null
                };
            }

            blog.Title = string.IsNullOrEmpty(request.Title) ? blog.Title : request.Title;
            blog.Content = string.IsNullOrEmpty(request.Content) ? blog.Content : _sanitizer.Sanitize(request.Content);
            blog.UpdateAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<Blog>().UpdateAsync(blog);

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Cập nhật blog thành công",
                    data = _mapper.Map<CreateTopicResponse>(blog)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Cập nhật blog thất bại",
                data = null
            };
        }
    }
}
