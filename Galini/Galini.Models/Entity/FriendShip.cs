using System;
using System.Collections.Generic;

namespace Galini.Models.Entity;

public partial class FriendShip
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid FriendId { get; set; }

    public string Status { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Account Friend { get; set; } = null!;

    public virtual Account User { get; set; } = null!;
}
