using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Request.UserPresence
{
    public class UpdateUserPresenceRequest
    {
        public bool Offline { get; set; }
        public bool Online { get; set; }
        public bool InCall { get; set; }
        public bool IsActive { get; set; }
    }
}
