using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Request.ListenerInfo
{
    public class CreateListenerInfoRequest
    {
        public string Description { get; set; } = null!;

        public decimal Price { get; set; }
    }
}
