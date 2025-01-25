using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Response
{
    public class BaseResponse
    {
        public string status { get; set; }
        public string? message { get; set; }
        public object? data { get; set; }
    }
}
