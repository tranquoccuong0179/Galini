using Galini.Models.Entity;
using Galini.Models.Payload.Request.Message;
using Galini.Models.Utils;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Galini.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;

namespace Galini.API.ConfigHub
{
    public sealed class ChatHub : Hub<IChatClient>
    {
        private static readonly ConcurrentDictionary<string, string> _userConnections = new(); // Lưu user online
        private readonly ILogger<ChatHub> _logger;
        private readonly IMessageService _messageService;
        private readonly IUnitOfWork<HarmonContext> _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;

        public ChatHub(IMessageService messageService, ILogger<ChatHub> logger, IUnitOfWork<HarmonContext> unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _messageService = messageService;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _logger.LogInformation("ChatHub initialized!");
            _contextAccessor = contextAccessor;
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine("===> OnConnectedAsync STARTED");

            var httpContext = _contextAccessor.HttpContext;
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

            Console.WriteLine($"===> Token: {token}");

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Token is missing from request headers or query string.");
                return;
            }

            var userId = GetUserIdFromToken(token);
            Console.WriteLine($"===> User ID: {userId}");

            if (!userId.HasValue)
            {
                _logger.LogWarning("Failed to retrieve user ID. Token: {Token}", token);
                return;
            }

            var username = Context.GetHttpContext()?.Request.Query["username"].ToString().Trim();
            Console.WriteLine($"===> Username: {username}");

            if (!Guid.TryParse(username, out Guid otherId))
            {
                _logger.LogWarning("Invalid username format: {Username}", username);
                return;
            }

            Console.WriteLine($"===> Parsed OtherId: {otherId}");

            var directChat = await GetDirectChat(userId.Value, otherId);
            Console.WriteLine("===> GetDirectChat Completed");

            await base.OnConnectedAsync();
            Console.WriteLine("===> OnConnectedAsync FINISHED");
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


        private async Task<DirectChat> GetDirectChat(Guid callerId, Guid otherId)
        {
            _logger.LogInformation($"Checking DirectChat for: {callerId} & {otherId}");

            var directChat = await _unitOfWork.GetRepository<DirectChat>()
                .SingleOrDefaultAsync(
                predicate: d => d.DirectChatParticipants.Any(dc => dc.AccountId.Equals(callerId))
                            && d.DirectChatParticipants.Any(dc => dc.AccountId.Equals(otherId))
                            && d.IsActive);

            _logger.LogInformation($"Error: {callerId} & {otherId}");


            if (directChat != null)
            {
                return directChat;
            }
            var newDirectChat = new DirectChat
            {
                Id = Guid.NewGuid(),
                Name = $"Chat-{callerId}-{otherId}",
                IsActive = true,
                CreateAt = TimeUtil.GetCurrentSEATime(),
                UpdateAt = TimeUtil.GetCurrentSEATime()
            };

            var participants = new List<DirectChatParticipant>
            {
                new DirectChatParticipant
                {
                    Id = Guid.NewGuid(),
                    AccountId = callerId,
                    DirectChatId = newDirectChat.Id, 
                    IsActive = true,
                    CreateAt = TimeUtil.GetCurrentSEATime(),
                    UpdateAt = TimeUtil.GetCurrentSEATime()
                },
                new DirectChatParticipant
                {
                    Id = Guid.NewGuid(),
                    AccountId = otherId,
                    DirectChatId = newDirectChat.Id,
                    IsActive = true,
                    CreateAt = TimeUtil.GetCurrentSEATime(),
                    UpdateAt = TimeUtil.GetCurrentSEATime()
                }
            };

            newDirectChat.DirectChatParticipants = participants;

            await _unitOfWork.GetRepository<DirectChat>().InsertAsync(newDirectChat);
            await _unitOfWork.CommitAsync();

            return newDirectChat;
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = _userConnections.FirstOrDefault(x => x.Value == Context.ConnectionId);
            if (!string.IsNullOrEmpty(user.Key))
            {
                _userConnections.TryRemove(user.Key, out _);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(Guid directChatId, string sender, string receiver, string message)
        {
            var createMessageRequest = new CreateMessageRequest { Content = message };
            var response = await _messageService.CreateMessage(createMessageRequest, directChatId);          

            if (response.status == "200")
            {
                // Nếu người nhận online -> gửi ngay
                if (_userConnections.TryGetValue(receiver, out var connectionId))
                {
                    await Clients.Client(connectionId).ReceiveMessage(sender, message);
                }
            }
            else
            {
                // Gửi lỗi về cho sender nếu lưu tin nhắn thất bại
                await Clients.Caller.ErrorMessage("Không thể gửi tin nhắn");
            }
        }

        public async Task GetOnlineUsers()
        {
            var users = _userConnections.Keys.ToList();

            await Clients.Caller.OnlineUsers(users);
        }
    }
}
