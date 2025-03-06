using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Request.DirectChatParticipant
{
    public class CreateDirectChatParticipant
    {
        public Guid DirectChatId { get; set; } 
        public string NickName { get; set; } = null!;
    }
}
