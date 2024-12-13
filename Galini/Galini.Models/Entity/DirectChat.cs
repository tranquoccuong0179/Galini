using System;
using System.Collections.Generic;

namespace Galini.Models.Entity;

public partial class DirectChat
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual ICollection<DirectChatParticipant> DirectChatParticipants { get; set; } = new List<DirectChatParticipant>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
