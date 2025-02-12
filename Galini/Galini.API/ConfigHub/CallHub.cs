﻿using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.SignalR;

namespace Galini.API.ConfigHub
{
    public class CallHub : Hub
    {
        private readonly IUserStatusService _userStatusService;

        public CallHub(IUserStatusService userStatusService)
        {
            _userStatusService = userStatusService;
        }

        public override async Task OnConnectedAsync()
        {
            await _userStatusService.AddUser(Context.ConnectionId);  // Khi user kết nối -> Thêm vào danh sách rảnh
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await _userStatusService.RemoveUser(Context.ConnectionId); // Khi user ngắt kết nối -> Xóa khỏi danh sách
        }

        public async Task GetRandomUser()
        {
            var targetConnectionId = await _userStatusService.GetRandomUser(Context.ConnectionId);

            if (!string.IsNullOrEmpty(targetConnectionId))
            {
                await Clients.Client(Context.ConnectionId).SendAsync("RandomUserSelected", targetConnectionId);
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

        public async Task AcceptCall(string callerConnectionId)
        {
            await Clients.Client(callerConnectionId).SendAsync("CallAccepted", Context.ConnectionId);
            await Clients.Client(Context.ConnectionId).SendAsync("CallAccepted", callerConnectionId);

            await _userStatusService.RemoveUser(callerConnectionId);
            await _userStatusService.RemoveUser(Context.ConnectionId);
        }

        public async Task RejectCall(string callerConnectionId)
        {
            await Clients.Client(callerConnectionId).SendAsync("CallRejected");
            await _userStatusService.AddUser(callerConnectionId);
        }

        public async Task EndCall()
        {
            await _userStatusService.AddUser(Context.ConnectionId);
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
