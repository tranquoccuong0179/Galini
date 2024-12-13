using System;
using System.Collections.Generic;

namespace Galini.Models.Entity;

public partial class DirectChatParticipant
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public Guid DirectChatId { get; set; }

    public string? NickName { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual DirectChat DirectChat { get; set; } = null!;
}
