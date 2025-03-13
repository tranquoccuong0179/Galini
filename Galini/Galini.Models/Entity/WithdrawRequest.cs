using System;
using System.Collections.Generic;

namespace Galini.Models.Entity;

public partial class WithdrawRequest
{
    public Guid Id { get; set; }

    public Guid ListenerId { get; set; }

    public Guid AdminId { get; set; }

    public decimal Amount { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }
}
