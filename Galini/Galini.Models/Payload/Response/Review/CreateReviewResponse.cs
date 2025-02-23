using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Response.Review
{
    public class CreateReviewResponse
    {       
        public string? ReviewMessage { get; set; }
        public string? ReplyMessage { get; set; }
        public double Star { get; set; }
        public string? ListenerName { get; set; }
    }
}
