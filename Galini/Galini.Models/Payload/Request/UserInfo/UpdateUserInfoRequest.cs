using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Request.UserInfo
{
    public class UpdateUserInfoRequest
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public bool IsActive { get; set; }
    }
}
