using Galini.Models.Entity;
using Galini.Models.Utils;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Client;
using System.IdentityModel.Tokens.Jwt;

namespace Galini.API.ConfigHub
{
    public class CallBookingHub : Hub
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IUserStatusService _userStatusService;

        private readonly IUnitOfWork<HarmonContext> _unitOfWork;

        private readonly ILogger<ChatHub> _logger;


        public CallBookingHub(ILogger<ChatHub> logger, IUnitOfWork<HarmonContext> unitOfWork, IUserStatusService userStatusService, IHttpContextAccessor httpContextAccessor)
        {
            _userStatusService = userStatusService;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                _logger.LogWarning("HttpContext is null. Connection cannot proceed.");
                return;
            }

            var token = httpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token))
            {
                token = httpContext.Request.Query["access_token"];
            }

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Token is missing from request headers or query string.");
                return;
            }

            var userId = GetUserIdFromToken(token);

            if (!userId.HasValue)
            {
                _logger.LogWarning("Failed to retrieve user ID. Token: {Token}", token);
                Context.Abort();
                return;
            }

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(userId) && x.IsActive);
            if (account == null)
            {
                Context.Abort(); 
                return;
            }
            await _userStatusService.AddUserForBooking(userId.ToString(), Context.ConnectionId);  // Khi user của booking kết nối -> Thêm vào danh sách theo accounId
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

        public async Task AcceptCall(string accountId1, string callerConnectionId)
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
            await Clients.Client(callerConnectionId).SendAsync("CallAccepted", Context.ConnectionId);
            await Clients.Client(Context.ConnectionId).SendAsync("CallAccepted", callerConnectionId);

            await _userStatusService.RemoveUserForBooking(accountId1, callerConnectionId);
            await _userStatusService.RemoveUserForBooking(accountId.ToString(), Context.ConnectionId);
        }

        public async Task RejectCall(string accountId1, string callerConnectionId) //accountId1 là người bị Reject, accountId2 là người bấm reject
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
            await Clients.Client(callerConnectionId).SendAsync("CallRejected");
            await _userStatusService.AddUserForBooking(accountId1, callerConnectionId);
            await _userStatusService.AddUserForBooking(accountId.ToString(), Context.ConnectionId);
        }

        public async Task EndCall(string accountId1, string callerConnectionId) //accountId1 là người bị Reject, accountId2 là người bấm reject
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
            await _userStatusService.AddUserForBooking(accountId1, callerConnectionId);
            await _userStatusService.AddUserForBooking(accountId.ToString(), Context.ConnectionId);
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

        private Guid? GetUserIdFromToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var accountIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "accountId");
                return accountIdClaim != null ? Guid.Parse(accountIdClaim.Value) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error decoding JWT token: {Message}", ex.Message);
                return null;
            }
        }
    }
}
