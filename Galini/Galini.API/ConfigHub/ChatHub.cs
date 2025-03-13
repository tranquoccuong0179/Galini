using Galini.Models.Payload.Request.Message;
using Galini.Services.Interface;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Galini.API.ConfigHub
{
    public sealed class ChatHub : Hub<IChatClient>
    {
        private static readonly ConcurrentDictionary<string, string> _userConnections = new(); // Lưu user online
        private readonly ILogger<ChatHub> _logger;
        private readonly IMessageService _messageService;

        public ChatHub(IMessageService messageService, ILogger<ChatHub> logger)
        {
            _messageService = messageService;
            _logger = logger;
            _logger.LogInformation("ChatHub initialized!");
        }

        public override async Task OnConnectedAsync()
        {
            var username = Context.GetHttpContext()?.Request.Query["username"];
            if (!string.IsNullOrEmpty(username))
            {
                _logger.LogInformation("Ping received!");
                _userConnections[username] = Context.ConnectionId;
            }
            else
            {
                _logger.LogWarning("User connected without username.");
            }
            await base.OnConnectedAsync();
        }

        public async Task Ping()
        {
            _logger.LogInformation("Ping received!");
            await Clients.Caller.ReceiveMessage("Server", "Pong! 🏓");
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
