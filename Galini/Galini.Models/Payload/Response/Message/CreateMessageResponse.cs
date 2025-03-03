using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Response.Message
{
    public class CreateMessageResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = null!;
        public string Type { get; set; } = null!;
    }
}
