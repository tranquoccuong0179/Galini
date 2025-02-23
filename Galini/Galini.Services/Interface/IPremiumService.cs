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
        public Task<BaseResponse> GetAllPremium(int page, int size, int? friend, bool? timelimit, int? match, double? minPrice, double? maxPrice, bool? sortByFriend, bool? sortByMatch, bool? sortByPrice, bool? sortByTimelimit);
        public Task<BaseResponse> GetPremiumById(Guid premiumId);
        public Task<BaseResponse> UpdatePremium(Guid premiumId, UpdatePremiumRequest request);
        public Task<BaseResponse> RemovePremium(Guid premiumId);
    }
}
