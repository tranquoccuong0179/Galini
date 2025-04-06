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
using System.Linq;
using System.Threading.Tasks;

namespace Galini.API.ConfigHub
{
    public sealed class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> _userConnections = new();
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
            Console.WriteLine($"===> directChat ID: {directChat.Id}");

            var messages = await _unitOfWork.GetRepository<Message>()
                .GetListAsync(predicate: m => m.DirectChatId == directChat.Id,
                              orderBy: q => q.OrderBy(m => m.CreateAt));
            Console.WriteLine($"===> Found {messages.Count} messages");

            var senderIds = messages.Select(m => m.SenderId).Distinct().ToList();
            Console.WriteLine($"===> Sender IDs: {string.Join(", ", senderIds)}");

            var accounts = await _unitOfWork.GetRepository<Account>()
                .GetListAsync(predicate: a => senderIds.Contains(a.Id));
            Console.WriteLine($"===> Found {accounts.Count} accounts for sender IDs");

            var messagesWithFullName = messages.Select(m => new
            {
                m.SenderId,
                SenderFullName = accounts.FirstOrDefault(a => a.Id == m.SenderId)?.FullName ?? "Unknown",
                m.Content,
                m.CreateAt
            }).ToList();

            Console.WriteLine("===> Messages with FullName:");
            foreach (var msg in messagesWithFullName)
            {
                Console.WriteLine($"SenderId: {msg.SenderId}, SenderFullName: {msg.SenderFullName}, Content: {msg.Content}");
            }

            var currentAccount = await _unitOfWork.GetRepository<Account>()
                .SingleOrDefaultAsync(predicate: a => a.Id.Equals(userId.Value));
            Console.WriteLine($"===> Current User FullName: {currentAccount?.FullName ?? "Unknown"}");

            await base.OnConnectedAsync();
            Console.WriteLine("===> OnConnectedAsync FINISHED");
            await Groups.AddToGroupAsync(Context.ConnectionId, directChat.Id.ToString());
            await Clients.Caller.SendAsync("DirectChatId", directChat.Id.ToString());
            Context.Items["UserToken"] = token;
            await Clients.Caller.SendAsync("ReceiveMessageThread", messagesWithFullName);
            //await Clients.Caller.SendAsync("fullName", currentAccount?.FullName ?? "Unknown");
            Console.WriteLine("===> GetMessage Successful");
        }

        private Guid? GetUserIdFromToken(string token)
        {
            try
            {
                if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = token.Substring("Bearer ".Length).Trim();
                }

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

        public async Task<DirectChat> GetDirectChat(Guid callerId, Guid otherId)
        {
            Console.WriteLine($"===> GetDirectChat: Sender={callerId}, Receiver={otherId}");

            var directChat = await _unitOfWork.GetRepository<DirectChat>()
                .SingleOrDefaultAsync(
                    predicate: d => d.DirectChatParticipants.Any(dc => dc.AccountId.Equals(callerId))
                                && d.DirectChatParticipants.Any(dc => dc.AccountId.Equals(otherId))
                                && d.IsActive);


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

            Console.WriteLine("directChat thất bại");
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

        public async Task SendMessage(Guid directChatId, string message)
        {

            var token = Context.Items["UserToken"]?.ToString();
            Console.WriteLine("===> Token: " + token);

            if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = token.Substring("Bearer ".Length).Trim();
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
                return;
            }

            Console.WriteLine($"===> SendMessage called with: directChatId={directChatId}, sender={userId}, message={message}");

            if (directChatId == Guid.Empty || userId == Guid.Empty || string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine("===> Send Message Failed: Invalid parameters");
                return;
            }

            var newMessage = new Message
            {
                Id = Guid.NewGuid(),
                DirectChatId = directChatId,
                SenderId = userId.Value,
                Content = message,
                IsActive = true,
                Type = "text",
                CreateAt = TimeUtil.GetCurrentSEATime(),
                UpdateAt = TimeUtil.GetCurrentSEATime()
            };

            await _unitOfWork.GetRepository<Message>().InsertAsync(newMessage);
            if (await _unitOfWork.CommitAsync() > 0)
            {
                Console.WriteLine("===> Message Saved Successfully");

                var senderAccount = await _unitOfWork.GetRepository<Account>()
                    .SingleOrDefaultAsync(predicate: a => a.Id.Equals(userId.Value));
                var senderFullName = senderAccount?.FullName ?? "Unknown";
                Console.WriteLine($"===> Sender ID: {userId.Value}, Sender FullName: {senderFullName}");

                await Clients.Group(directChatId.ToString())
                    .SendAsync("NewMessage", userId.Value, message, senderFullName);
            }
            else
            {
                Console.WriteLine("===> Message Save Failed");
            }
        }


        public async Task GetOnlineUsers()
        {
            var users = _userConnections.Keys.ToList();
            await Clients.Caller.SendAsync("OnlineUsers", users);
        }
    }
}