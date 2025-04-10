﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Galini.Models.Entity;

public partial class UserInfo
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public Guid PremiumId { get; set; }

    public DateTime DateStart { get; set; }

    public DateTime DateEnd { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    [JsonIgnore]
    public virtual Account Account { get; set; } = null!;

    public virtual Premium Premium { get; set; } = null!;
}
