﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Response.Topic
{
    public class GetTopicResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}
