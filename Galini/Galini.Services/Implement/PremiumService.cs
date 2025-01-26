using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Payload.Request.Premium;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.Premium;
using Galini.Models.Payload.Response.UserInfo;
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
    public class PremiumService : BaseService<PremiumService>, IPremiumService
    {
        public PremiumService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<PremiumService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse> CreatePremium(CreatePremiumRequest request)
        {
            var premium = _mapper.Map<CreatePremiumRequest, Premium>(request);   
            
            await _unitOfWork.GetRepository<Premium>().InsertAsync(premium);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Tạo gói premium thành công",
                    data = _mapper.Map<CreatePremiumResponse>(premium)
                };
            }
            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Tạo gói premium thất bại",
                data = null
            };
        }

        public async Task<BaseResponse> GetAllPremium(int page, int size)
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

            var premium = await _unitOfWork.GetRepository<Premium>().GetPagingListAsync(
                selector: a => new CreatePremiumResponse
                {
                    Type = a.Type,
                    Friend = a.Friend,
                    Timelimit = a.Timelimit,
                    Match = a.Match
                },
                predicate: a => a.IsActive,
                page: page,
                size: size);

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy thông tin premium thành công",
                data = premium
            };
        }

        public async Task<BaseResponse> GetPremiumById(Guid premiumId)
        {
            var premium = await _unitOfWork.GetRepository<Premium>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(premiumId) && x.IsActive);

            if (premium == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin premium với ID này",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy thông tin premium thành công",
                data = _mapper.Map<CreatePremiumResponse>(premium)
            };
        }

        public async Task<BaseResponse> RemovePremium(Guid premiumId)
        {
            var premium = await _unitOfWork.GetRepository<Premium>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(premiumId) && x.IsActive);

            if (premium == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin premium với ID này",
                    data = null
                };
            }

            premium.IsActive = false;
            premium.DeleteAt = TimeUtil.GetCurrentSEATime(); 
            _unitOfWork.GetRepository<Premium>().UpdateAsync(premium);

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Xóa thông tin premium thành công",
                    data = isSuccessfully
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Xóa thông tin premium thất bại",
                data = isSuccessfully
            };
        }

        public async Task<BaseResponse> UpdatePremium(Guid premiumId, UpdatePremiumRequest request)
        {
            var premium = await _unitOfWork.GetRepository<Premium>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(premiumId) && x.IsActive);

            if (premium == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin premium với ID này",
                    data = null
                };
            }

            premium = _mapper.Map(request, premium);

            _unitOfWork.GetRepository<Premium>().UpdateAsync(premium);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Cập nhật thông tin premium thành công",
                    data = _mapper.Map<CreatePremiumResponse>(premium)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Cập nhật thông tin premium thất bại",
                data = null
            };
        }
    }
}
