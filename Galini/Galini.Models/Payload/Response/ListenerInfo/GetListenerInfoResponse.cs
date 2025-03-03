using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Response.ListenerInfo
{
    public class GetListenerInfoResponse
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public double? Star { get; set; }
        public decimal Price { get; set; }
        public string? FullName { get; set; }
    }
}
