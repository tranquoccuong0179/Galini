using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Io;
using Galini.Models.Payload.Request.Blog;
using Galini.Models.Payload.Response;
using Microsoft.AspNetCore.Http;

namespace Galini.Services.Interface
{
    public interface IBlogService
    {
        Task<BaseResponse> CreateBlog(CreateBlogRequest request);
        Task<BaseResponse> GetAllBlogs(int page, int size);
        Task<BaseResponse> GetBlogById(Guid id);
        Task<BaseResponse> RemoveBlog(Guid id);
        Task<BaseResponse> UpdateBlog(Guid id, UpdateBlogRequest request);
        Task<BaseResponse> UpImageForDescription(IFormFile formFile);
        Task<BaseResponse> LikeBlog(Guid id);
        Task<BaseResponse> GetAllBlogByUser();
    }
}
