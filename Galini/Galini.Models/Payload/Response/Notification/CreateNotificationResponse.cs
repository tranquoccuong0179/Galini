using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galini.Models.Enum;

namespace Galini.Models.Payload.Response.Notification
{
    public class CreateNotificationResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public TypeEnum Type { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
    }
}
