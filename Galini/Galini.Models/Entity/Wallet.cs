﻿using System;
using System.Collections.Generic;

namespace Galini.Models.Entity;

public partial class Wallet
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public decimal Balance { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
