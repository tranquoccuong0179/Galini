using Galini.Models.Entity;
using Galini.Models.Utils;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Client;

namespace Galini.API.ConfigHub
{
    public class CallBookingHub : Hub
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IUserStatusService _userStatusService;

        private readonly IUnitOfWork<HarmonContext> _unitOfWork;


        public CallBookingHub(IUnitOfWork<HarmonContext> unitOfWork, IUserStatusService userStatusService, IHttpContextAccessor httpContextAccessor)
        {
            _userStatusService = userStatusService;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        public override async Task OnConnectedAsync()
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            if (accountId == null)
            {
                Context.Abort(); // Ngắt kết nối nếu không có accountId
                return;
            }

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(accountId) && x.IsActive);
            if (account == null)
            {
                Context.Abort(); // Ngắt kết nối nếu tài khoản không tồn tại
                return;
            }
            await _userStatusService.AddUserForBooking(accountId.ToString(), Context.ConnectionId);  // Khi user của booking kết nối -> Thêm vào danh sách theo accounId
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            if (accountId == null)
            {
                Context.Abort(); // Ngắt kết nối nếu không có accountId
                return;
            }

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(accountId) && x.IsActive);
            if (account == null)
            {
                Context.Abort(); // Ngắt kết nối nếu tài khoản không tồn tại
                return;
            }
            await _userStatusService.RemoveUserForBooking(account.Id.ToString(), Context.ConnectionId); // Khi user ngắt kết nối -> Xóa khỏi danh sách
        }

        public async Task GetUserForBooking(string accountId) // Tìm connectionId của 1 người trong book (user hoặc listener) dựa vào accountId
        {
            var targetConnectionId = await _userStatusService.GetUserForBooking(accountId);

            if (!string.IsNullOrEmpty(targetConnectionId))
            {
                await Clients.Client(Context.ConnectionId).SendAsync("UserSelected", targetConnectionId);
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("NoAvailableUsers");
            }
        }

        public async Task StartCall(string targetConnectionId)
        {
            if (!string.IsNullOrEmpty(targetConnectionId))
            {
                await Clients.Client(targetConnectionId).SendAsync("IncomingCall", Context.ConnectionId);
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("NoAvailableUsers");
            }
        }

        public async Task AcceptCall(string accountId1, string accountId2, string callerConnectionId)
        {
            await Clients.Client(callerConnectionId).SendAsync("CallAccepted", Context.ConnectionId);
            await Clients.Client(Context.ConnectionId).SendAsync("CallAccepted", callerConnectionId);

            await _userStatusService.RemoveUserForBooking(accountId1, callerConnectionId);
            await _userStatusService.RemoveUserForBooking(accountId2, Context.ConnectionId);
        }

        public async Task RejectCall(string accountId1, string accountId2, string callerConnectionId) //accountId1 là người bị Reject, accountId2 là người bấm reject
        {
            await Clients.Client(callerConnectionId).SendAsync("CallRejected");
            await _userStatusService.AddUserForBooking(accountId1, callerConnectionId);
            await _userStatusService.AddUserForBooking(accountId2, Context.ConnectionId);
        }

        public async Task EndCall(string accountId1, string accountId2, string callerConnectionId) //accountId1 là người bị Reject, accountId2 là người bấm reject
        {
            await _userStatusService.AddUserForBooking(accountId1, callerConnectionId);
            await _userStatusService.AddUserForBooking(accountId2, Context.ConnectionId);
            await Clients.Client(Context.ConnectionId).SendAsync("CallEnded"); // Thông báo về FE
        }

        public async Task SendOffer(string targetConnectionId, string offer)
        {
            await Clients.Client(targetConnectionId).SendAsync("ReceiveOffer", Context.ConnectionId, offer);
        }

        public async Task SendAnswer(string targetConnectionId, string answer)
        {
            await Clients.Client(targetConnectionId).SendAsync("ReceiveAnswer", Context.ConnectionId, answer);
        }

        public async Task SendCandidate(string targetConnectionId, string candidate)
        {
            await Clients.Client(targetConnectionId).SendAsync("ReceiveCandidate", Context.ConnectionId, candidate);
        }
    }
}
