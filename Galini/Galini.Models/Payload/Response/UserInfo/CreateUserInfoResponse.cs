using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Response.UserInfo
{
    public class CreateUserInfoResponse
    {
        public Guid AccountId { get; set; }
        public Guid PremiumId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }         
    }
}
