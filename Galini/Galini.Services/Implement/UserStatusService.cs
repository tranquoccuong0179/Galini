using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Utils;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Implement
{
    public class UserStatusService : BaseService<UserStatusService>, IUserStatusService
    {
        private readonly IConnectionMultiplexer _redis;
        private const string UsersKey = "AvailableUsers";

        public UserStatusService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<UserStatusService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConnectionMultiplexer redis) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _redis = redis;
        }

        public async Task AddUser(string connectionId)
        {
            var db = _redis.GetDatabase();
            await db.SetAddAsync(UsersKey, connectionId); 
        }

        public async Task RemoveUser(string connectionId)
        {
            var db = _redis.GetDatabase();
            await db.SetRemoveAsync(UsersKey, connectionId); 
        }

        public async Task RemoveUserForBooking(string accountId, string connectionId)
        {
            var db = _redis.GetDatabase();
            await db.SetRemoveAsync(accountId, connectionId);
        }

        public async Task<string?> GetRandomUser(string currentConnectionId)
        {
            var db = _redis.GetDatabase();
            var users = await db.SetMembersAsync(UsersKey);

            // Lọc bỏ chính connectionId của người gọi
            var availableUsers = users
                .Select(u => u.ToString())
                .Where(u => u != currentConnectionId)
                .ToList();

            if (availableUsers.Count == 0)
            {
                return null; // Không có người dùng nào khác
            }

            var random = new Random();
            return availableUsers[random.Next(availableUsers.Count)];
        }

        public async Task AddUserForBooking(string accountId, string connectionId)
        {
            var db = _redis.GetDatabase();
            await db.SetAddAsync(accountId, connectionId);
        }

        public async Task<string?> GetUserForBooking(string accountId)
        {
            var db = _redis.GetDatabase();
            var users = await db.SetMembersAsync(accountId);

            // Nếu tập hợp trống, trả về null
            if (users.Length == 0)
            {
                return null;
            }

            // Trả về bất kỳ connectionId nào trong danh sách
            return users[0].ToString();
        }
    }
}
