using System;
using System.Collections.Generic;

namespace Galini.Models.Entity;

public partial class Message
{
    public Guid Id { get; set; }

    public Guid DirectChatId { get; set; }

    public Guid SenderId { get; set; }

    public string Content { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual DirectChat DirectChat { get; set; } = null!;

    public virtual Account Sender { get; set; } = null!;
}
