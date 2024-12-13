using System;
using System.Collections.Generic;

namespace Galini.Models.Entity;

public partial class UserInfo
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public Guid PremiumId { get; set; }

    public int TimeRemaining { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Premium Premium { get; set; } = null!;
}
