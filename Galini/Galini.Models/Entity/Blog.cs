using System;
using System.Collections.Generic;

namespace Galini.Models.Entity;

public partial class Blog
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string Type { get; set; } = null!;

    public int? Views { get; set; }

    public int? Likes { get; set; }

    public string Status { get; set; } = null!;

    public Guid AuthorId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Account Author { get; set; } = null!;
}
