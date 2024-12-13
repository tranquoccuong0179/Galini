using System;
using System.Collections.Generic;

namespace Galini.Models.Entity;

public partial class UserCall
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public Guid CallHistoryId { get; set; }

    public string CallRole { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual CallHistory CallHistory { get; set; } = null!;
}
