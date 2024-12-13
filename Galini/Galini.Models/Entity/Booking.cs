using System;
using System.Collections.Generic;

namespace Galini.Models.Entity;

public partial class Booking
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid WorkShiftId { get; set; }

    public Guid ListenerId { get; set; }

    public DateTime Date { get; set; }

    public string Status { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Review? Review { get; set; }

    public virtual Account User { get; set; } = null!;

    public virtual WorkShift WorkShift { get; set; } = null!;
}
