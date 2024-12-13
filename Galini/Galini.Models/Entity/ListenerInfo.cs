using System;
using System.Collections.Generic;

namespace Galini.Models.Entity;

public partial class ListenerInfo
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public string Description { get; set; } = null!;

    public double? Star { get; set; }

    public decimal Price { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();
}
