using System;
using System.Collections.Generic;

namespace Galini.Models.Entity;

public partial class CallHistory
{
    public Guid Id { get; set; }

    public DateTime TimeStart { get; set; }

    public DateTime TimeEnd { get; set; }

    public int Duration { get; set; }

    public bool IsMissCall { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual ICollection<UserCall> UserCalls { get; set; } = new List<UserCall>();
}
