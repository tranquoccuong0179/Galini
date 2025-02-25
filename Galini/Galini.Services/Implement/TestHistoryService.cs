using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Payload.Request.TestHistory;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.Review;
using Galini.Models.Payload.Response.TestHistory;
using Galini.Models.Payload.Response.Topic;
using Galini.Models.Utils;
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
    public class TestHistoryService : BaseService<TestHistoryService>, ITestHistoryService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public TestHistoryService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<TestHistoryService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<BaseResponse> CreateTestHistory(CreateTestHistoryRequest request)
        {
            Guid? id = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: l => l.Id.Equals(id) && l.IsActive == true);

            if (account == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy người nghe",
                    data = null
                };
            }

            var testHistory = _mapper.Map<CreateTestHistoryRequest, TestHistory>(request);
            testHistory.AccountId = account.Id;

            await _unitOfWork.GetRepository<TestHistory>().InsertAsync(testHistory);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Thêm lịch sử kiểm tra thành công",
                    data = _mapper.Map<CreateTopicResponse>(testHistory)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Thêm lịch sử kiểm tra thất bại",
                data = null
            };
        }

        public async Task<BaseResponse> GetAllTestHistory(int page, int size, int? grade, string? status, bool? sortByGrade)
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

            var testHistory = await _unitOfWork.GetRepository<TestHistory>().GetPagingListAsync(
                selector: a => _mapper.Map<CreateTestHistoryResponse>(a),
                predicate: a => a.IsActive && (!grade.HasValue || a.Grade >= grade) && (string.IsNullOrEmpty(status) || a.Status.Equals(status)),
                orderBy: l => sortByGrade.HasValue ? (sortByGrade.Value ? l.OrderBy(l => l.Grade) : l.OrderByDescending(l => l.Grade)) : l.OrderBy(l => l.CreateAt),
                page: page,
                size: size);

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy lịch sử kiểm tra thành công",
                data = testHistory
            };
        }

        public async Task<BaseResponse> GetTestHistoryByAccountId(int page, int size, int? grade, string? status, bool? sortByGrade)
        {
            Guid? id = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: l => l.Id.Equals(id) && l.IsActive == true);

            if (account == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy người nghe",
                    data = null
                };
            }

            if (page < 1 || size < 1)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Page hoặc size không hợp lệ.",
                    data = null
                };
            }

            var testHistory = await _unitOfWork.GetRepository<TestHistory>().GetPagingListAsync(
                selector: a => _mapper.Map<CreateTestHistoryResponse>(a),
                predicate: a => a.IsActive && (!grade.HasValue || a.Grade >= grade) && (string.IsNullOrEmpty(status) || a.Status.Equals(status) && a.AccountId.Equals(account.Id)),
                orderBy: l => sortByGrade.HasValue ? (sortByGrade.Value ? l.OrderBy(l => l.Grade) : l.OrderByDescending(l => l.Grade)) : l.OrderBy(l => l.CreateAt),
                page: page,
                size: size);

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy lịch sử kiểm tra thành công",
                data = testHistory
            };
        }

        public async Task<BaseResponse> GetTestHistoryById(Guid testHistoryId)
        {
            var testHistory = await _unitOfWork.GetRepository<TestHistory>().SingleOrDefaultAsync(
                selector: t => _mapper.Map<CreateTestHistoryResponse>(t),
                predicate: t => t.Id.Equals(testHistoryId) && t.IsActive == true);

            if (testHistory == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy lịch sử kiểm tra này",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy lịch sử kiểm tra thành công",
                data = testHistory
            };
        }

        public async Task<BaseResponse> RemoveTestHistory(Guid testHistoryId)
        {
            var testHistory = await _unitOfWork.GetRepository<TestHistory>().SingleOrDefaultAsync(
                predicate: t => t.Id.Equals(testHistoryId) && t.IsActive == true);

            if (testHistory == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy lịch sử kiểm tra này",
                    data = null
                };
            }

            testHistory.IsActive = false;
            testHistory.DeleteAt = TimeUtil.GetCurrentSEATime();
            testHistory.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<TestHistory>().UpdateAsync(testHistory);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Xóa lịch sử kiểm tra thành công",
                    data = true
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Xóa lịch sử kiểm tra thất bại",
                data = false
            };
        }

        public async Task<BaseResponse> UpdateTestHistory(Guid testHistoryId, UpdateTestHistoryRequest request)
        {
            var testHistory = await _unitOfWork.GetRepository<TestHistory>().SingleOrDefaultAsync(
                predicate: t => t.Id.Equals(testHistoryId) && t.IsActive == true);

            if (testHistory == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy lịch sử kiểm tra này",
                    data = null
                };
            }

            _mapper.Map(request, testHistory);
            _unitOfWork.GetRepository<TestHistory>().UpdateAsync(testHistory);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Cập nhật lịch sử kiểm tra thành công",
                    data = _mapper.Map<CreateTestHistoryResponse>(testHistory)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Cập nhật lịch sử kiểm tra thất bại",
                data = null
            };
        }
    }
}
