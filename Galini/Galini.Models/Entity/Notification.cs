using System;
using System.Collections.Generic;

namespace Galini.Models.Entity;

public partial class Notification
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Type { get; set; } = null!;

    public string Content { get; set; } = null!;

    public bool IsRead { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Account User { get; set; } = null!;
}
