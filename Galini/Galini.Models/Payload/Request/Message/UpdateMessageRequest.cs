using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Request.Message
{
    public class UpdateMessageRequest
    {
        public string? Content { get; set; } 
        public bool IsActive { get; set; }
    }
}
