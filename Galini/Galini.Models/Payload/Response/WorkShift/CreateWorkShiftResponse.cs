using Galini.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Response.WorkShift
{
    public class CreateWorkShiftResponse
    {
        public Guid Id { get; set; }
        public DayEnum Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
