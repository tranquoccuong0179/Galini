using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galini.Models.Entity;

namespace Galini.Services.Interface
{
    public interface IChatClient
    {
        Task ReceiveMessage(string message);
        Task ReceiveMessageThread(IEnumerable<Message> messages);
        Task ReceiveMessage(string sender, string message);
        Task ErrorMessage(string error);
        Task OnlineUsers(IEnumerable<string> users);
    }
}
