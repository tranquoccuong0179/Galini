using System;
using System.Collections.Generic;

namespace Galini.Models.Entity;

public partial class Account
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string Gender { get; set; } = null!;

    public string? AvatarUrl { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();

    public virtual ICollection<DirectChatParticipant> DirectChatParticipants { get; set; } = new List<DirectChatParticipant>();

    public virtual ICollection<FriendShip> FriendShipFriends { get; set; } = new List<FriendShip>();

    public virtual ICollection<FriendShip> FriendShipUsers { get; set; } = new List<FriendShip>();

    public virtual ListenerInfo? ListenerInfo { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<TestHistory> TestHistories { get; set; } = new List<TestHistory>();

    public virtual ICollection<UserCall> UserCalls { get; set; } = new List<UserCall>();

    public virtual UserInfo? UserInfo { get; set; }

    public virtual UserPresence? UserPresence { get; set; }

    public virtual Wallet? Wallet { get; set; }

    public virtual ICollection<WorkShift> WorkShifts { get; set; } = new List<WorkShift>();
}
