using Galini.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Request.FriendShip
{
    public class UpdateFriendShipRequest
    {
        public FriendShipEnum Status { get; set; }
        public bool IsActive { get; set; }
    }
}
