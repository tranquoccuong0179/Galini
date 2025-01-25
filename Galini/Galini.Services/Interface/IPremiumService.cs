using Galini.Models.Payload.Request.Premium;
using Galini.Models.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface IPremiumService
    {
        public Task<BaseResponse> CreatePremium(CreatePremiumRequest request);
        public Task<BaseResponse> GetAllPremium(int page, int size);
        public Task<BaseResponse> GetPremiumById(Guid premiumId);
        public Task<BaseResponse> UpdatePremium(Guid premiumId, CreatePremiumRequest request);
        public Task<BaseResponse> RemovePremium(Guid premiumId);
    }
}
