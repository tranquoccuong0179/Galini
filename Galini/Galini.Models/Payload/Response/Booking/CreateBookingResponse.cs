using Galini.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Response.Booking
{
    public class CreateBookingResponse
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public BookingEnum Status { get; set; }
        public string ListenerName { get; set; }
        public string UserName { get; set; }
        public string Time { get; set; }
    }
}
