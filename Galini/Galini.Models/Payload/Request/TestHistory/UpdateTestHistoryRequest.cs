using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Request.TestHistory
{
    public class UpdateTestHistoryRequest
    {
        public int Grade { get; set; }

        public string Status { get; set; }

        public bool IsActive { get; set; }
    }
}
