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
        public string FullNameFriend { get; set; } = null!;
        public string EmailFriend { get; set; } = null!;
        public string PhoneFriend { get; set; } = null!;
        public DateTime DateOfBirthFriend { get; set; }
        public string? AvatarUrlFriend { get; set; }
        public string GenderFriend { get; set; } = null!;
        public Guid PremiumIdFriend { get; set; }
        public DateTime DateStartFriend { get; set; }
        public DateTime DateEndFriend { get; set; }
        public string Status { get; set; } = null!;
    }
}
