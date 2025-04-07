using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Response.DirectChat
{
    public class GetDirectChatResponse
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? LatestMessage { get; set; }
        public Entity.Account? Friend {  get; set; }
    }
}
