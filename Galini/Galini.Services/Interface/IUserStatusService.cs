using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface IUserStatusService
    {
        public Task AddUser(string connectionId);

        public Task RemoveUser(string connectionId);

        public Task<string?> GetRandomUser(string currentConnectionId);

        public Task AddUserForBooking(string connectionId, string accountId);

        public Task<string?> GetUserForBooking(string currentConnectionId, string accountId);
    }
}
