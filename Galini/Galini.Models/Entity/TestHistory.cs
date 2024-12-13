using System;
using System.Collections.Generic;

namespace Galini.Models.Entity;

public partial class TestHistory
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public int Grade { get; set; }

    public string Status { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Account Account { get; set; } = null!;
}
