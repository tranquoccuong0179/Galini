using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Galini.Models.Enum;

namespace Galini.Models.Payload.Request.Notification
{
    public class CreateNotificationRequest
    {
        
        public TypeEnum Type { get; set; } 

        public string Content { get; set; } = null!;
    }
}
