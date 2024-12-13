using System;
using System.Collections.Generic;

namespace Galini.Models.Entity;

public partial class UserPresence
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public bool Offline { get; set; }

    public bool Online { get; set; }

    public bool InCall { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Account Account { get; set; } = null!;
}
