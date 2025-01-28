using Galini.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Response.UserCall
{
    public class CreateUserCallResponse
    {
        public Guid AccountId { get; set; }
        public Guid CallHistoryId { get; set; }
        public CallRoleEnum CallRole { get; set; }
    }
}
