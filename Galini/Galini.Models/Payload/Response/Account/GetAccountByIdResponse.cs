using Galini.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Response.Account
{
    public class GetAccountByIdResponse
    {
        public Guid Id { get; set; }

        public string Role { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; } = null!;

        public int? Duration { get; set; }

        public string? AvatarUrl { get; set; }

        public FriendShipEnum Status { get; set; }

        public Entity.UserInfo UserInfo { get; set; }
    }
}
