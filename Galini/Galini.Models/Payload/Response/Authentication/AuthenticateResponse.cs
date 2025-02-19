using Galini.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Response.Authentication
{
    public class AuthenticateResponse
    {
        public string token { get; set; }
        public Guid AccountId { get; set; }
        public string UserName { get; set; }
        public string RoleEnum { get; set; }
        public string RefreshToken { get; set; }
    }
}
