﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Response.Wallet
{
    public class GetWalletResponse
    {
        public Guid Id { get; set; }
        public decimal? Balance { get; set; }
    }
}
