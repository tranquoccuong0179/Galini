using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AngleSharp.Io;
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
        private const string FirebaseStorageBaseUrl = "https://firebasestorage.googleapis.com/v0/b/test-ce15e.appspot.com/o";
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

        private string ParseDownloadUrl(string responseBody, string fileName)
        {
            var json = JsonDocument.Parse(responseBody);
            var nameElement = json.RootElement.GetProperty("name");
            var downloadUrl = $"{FirebaseStorageBaseUrl}/{Uri.EscapeDataString(nameElement.GetString())}?alt=media";
            return downloadUrl;
        }

        public async Task<BaseResponse> UpImageForDescription(IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return new BaseResponse
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "File is null or empty",
                    data = null
                };
            }
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var allowedContentTypes = new[] { "image/jpeg", "image/png" };
            if (!allowedContentTypes.Contains(formFile.ContentType, StringComparer.OrdinalIgnoreCase) ||
    !allowedExtensions.Contains(Path.GetExtension(formFile.FileName), StringComparer.OrdinalIgnoreCase))
            {
                return new BaseResponse
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Only .jpg, .jpeg, and .png files are allowed",
                    data = null
                };
            }

            long maxFileSize = 300 * 1024;
            if (formFile.Length > maxFileSize)
            {
                return new BaseResponse
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "File size must not exceed 300 KB",
                    data = null
                };
            }

            try
            {
                using (var client = new HttpClient())
                {
                    string fileName = Path.GetFileName(formFile.FileName);
                    string firebaseStorageUrl = $"{FirebaseStorageBaseUrl}?uploadType=media&name=image/{Guid.NewGuid()}_{fileName}";

                    using (var stream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(stream);
                        stream.Position = 0;

                        var content = new ByteArrayContent(stream.ToArray());
                        content.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);

                        var response = await client.PostAsync(firebaseStorageUrl, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseBody = await response.Content.ReadAsStringAsync();
                            var dowloadUrl = ParseDownloadUrl(responseBody, fileName);
                            return new BaseResponse()
                            {
                                status = StatusCodes.Status200OK.ToString(),
                                message = "Upload image successful",
                                data = dowloadUrl
                            };
                        }
                        else
                        {
                            var errorMessage = $"Error uploading file {fileName} to Firebase Storage. Status Code: {response.StatusCode}\nContent: {await response.Content.ReadAsStringAsync()}";
                            throw new Exception(errorMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while uploading the file to Firebase.", ex);
            }
        }
    }
}
