using Galini.Models.Enum;
using Galini.Models.Payload.Request.FriendShip;
using Galini.Models.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface IFriendShipService
    {
        public Task<BaseResponse> CreateFriendShip(Guid userId, Guid friendId);
        public Task<BaseResponse> GetAllFriendShip(int page, int size, string? status, bool? sortByStatus);
        public Task<BaseResponse> GetFriendList(int page, int size);
        public Task<BaseResponse> GetRequestList(int page, int size);
        public Task<BaseResponse> GetFriendShipById(Guid friendShipId);
        public Task<BaseResponse> GetFriendShipByAccountIdAndStatus(Guid accountId, FriendShipEnum status, int page, int size);
        public Task<BaseResponse> GetFriendByAccountId(Guid accountId, int page, int size);
        public Task<BaseResponse> UpdateFriendShip(Guid friendShipId, UpdateFriendShipRequest request);
        public Task<BaseResponse> RemoveFriendShip(Guid friendShipId);
        public Task<BaseResponse> SearchFriendByPhone(string phoneNumber);
    }
}
