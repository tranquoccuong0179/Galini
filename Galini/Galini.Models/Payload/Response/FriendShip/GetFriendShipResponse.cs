using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Response.FriendShip
{
    public class GetFriendShipResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid FriendId { get; set; }
        public string FriendFullName { get; set; } = null!;
        public DateTime FriendDateOfBirth { get; set; }
        public string FriendGender { get; set; } = null!;
        public string? FriendAvatarUrl { get; set; }
        public string Status { get; set; } = null!;
    }
}
