﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Response.Question
{
    public class CreateQuestionResponse
    {
        public Guid? Id { get; set; }

        public string? Content { get; set; } 
    }
}
