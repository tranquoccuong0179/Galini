using System;
using System.Collections.Generic;

namespace Galini.Models.Entity;

public partial class Review
{
    public Guid Id { get; set; }

    public Guid BookingId { get; set; }

    public Guid ListenerId { get; set; }

    public string? ReviewMessage { get; set; }

    public string? ReplyMessage { get; set; }

    public double Star { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Booking Booking { get; set; } = null!;
}
