using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Response.DirectChatParticipant
{
    public class CreateDirectChatParticipantResponse
    {
        public Guid? id {  get; set; }
        public Guid? AccountId { get; set; }
        public Guid? DirectChatId { get; set; }
        public string? NickName { get; set; }
    }
}
