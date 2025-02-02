using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galini.Models.Payload.Request.User;

namespace Galini.Models.Payload.Request.ListenerInfo
{
    public class CreateListenerInfoModel
    {
        public RegisterUserRequest RegisterRequest { get; set; }
        public CreateListenerInfoRequest ListenerRequest { get; set; }
    }
}
