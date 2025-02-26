using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Response.UserPresence
{
    public class CreateUserPresenceResponse
    {
        public bool Offline { get; set; }
        public bool Online { get; set; }
        public bool InCall { get; set; }
    }
}
