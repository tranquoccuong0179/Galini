using System;
using System.Collections.Generic;

namespace Galini.Models.Entity;

public partial class Deposit
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public string Code { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Amount { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Transaction? Transaction { get; set; }
}
