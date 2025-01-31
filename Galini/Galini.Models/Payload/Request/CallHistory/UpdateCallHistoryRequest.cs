using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Request.CallHistory
{
    public class UpdateCallHistoryRequest
    {
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public int Duration { get; set; }
        public bool IsMissCall { get; set; }
        public bool IsActive { get; set; }
    }
}
