using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Request.ListenerInfo
{
    public class UpdateListenerInfoRequest
    {
        public string? Description { get; set; }

        public decimal Price { get; set; }
    }
}
