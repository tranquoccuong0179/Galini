﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Request.Premium
{
    public class CreatePremiumRequest
    {
        public string Type { get; set; } = null!;
        public int Friend { get; set; }
        public bool Timelimit { get; set; }
        public int Match { get; set; }
        public double Price { get; set; }

    }
}
