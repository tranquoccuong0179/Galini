using Galini.Models.Request.FriendShip;
using Galini.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface IFriendShipService
    {
        public Task<BaseResponse> CreateFriendShip(CreateFriendShipRequest request, Guid userId, Guid friendId);
        public Task<BaseResponse> GetAllFriendShip(int page, int size);
        public Task<BaseResponse> GetFriendShipById(Guid friendShipId);
        public Task<BaseResponse> GetFriendByAccountId(Guid accountId);
        public Task<BaseResponse> GetBlockListByAccountId(Guid accountId);
        public Task<BaseResponse> GetFriendRequestByAccountId(Guid accountId);
        public Task<BaseResponse> GetSentFriendRequestByAccountId(Guid accountId);
        public Task<BaseResponse> UpdateFriendShip(Guid friendShipId, CreateFriendShipRequest request);
        public Task<BaseResponse> RemoveFriendShip(Guid friendShipId);
    }
}
