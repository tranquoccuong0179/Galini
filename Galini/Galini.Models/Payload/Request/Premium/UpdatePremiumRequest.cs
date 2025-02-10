using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Request.Premium
{
    public class UpdatePremiumRequest
    {
        public string? Type { get; set; } 
        public int Friend { get; set; }
        public bool Timelimit { get; set; }
        public int Match { get; set; }
        public bool IsActive { get; set; }
    }
}
