﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galini.Models.Enum;

namespace Galini.Models.Payload.Response.Topic
{
    public class CreateTopicResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}
