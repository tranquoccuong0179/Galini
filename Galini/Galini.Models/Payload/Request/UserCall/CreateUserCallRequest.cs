﻿using Galini.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Request.UserCall
{
    public class CreateUserCallRequest
    {
        public CallRoleEnum CallRole { get; set; }
    }
}
