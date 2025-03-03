using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galini.Models.Payload.Response.Account;

namespace Galini.Models.Payload.Response.ListenerInfo
{
    public class CreateListenerInfoResponse
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public double? Star { get; set; }
        public decimal? Price { get; set; }
        public RegisterUserResponse? Account {  get; set; } 
    }
}
