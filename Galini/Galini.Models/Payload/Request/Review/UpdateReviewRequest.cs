using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Request.Review
{
    public class UpdateReviewRequest
    {
        public string? ReviewMessage { get; set; }
        public string? ReplyMessage { get; set; }
        public double Star { get; set; }
    }
}
