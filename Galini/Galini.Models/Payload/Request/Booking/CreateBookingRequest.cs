﻿using Galini.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Request.Booking
{
    public class CreateBookingRequest
    {
        public DateTime Date { get; set; }

        public BookingEnum Status { get; set; }
    }
}
