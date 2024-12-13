using System;
using System.Collections.Generic;

namespace Galini.Models.Entity;

public partial class Question
{
    public Guid Id { get; set; }

    public string Content { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }
}
