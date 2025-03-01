using System;
using System.Collections.Generic;

namespace Galini.Models.Entity;

public partial class Topic
{
    public Guid Id { get; set; }

    public Guid ListenerInfoId { get; set; }

    public string Name { get; set; } = null!;

    public int Translate { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual ListenerInfo ListenerInfo { get; set; } = null!;
}
