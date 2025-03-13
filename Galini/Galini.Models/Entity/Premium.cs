using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Galini.Models.Entity;

public partial class Premium
{
    public Guid Id { get; set; }

    public string Type { get; set; } = null!;

    public int Friend { get; set; }

    public bool Timelimit { get; set; }

    public int Match { get; set; }

    public double Price { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }
    [JsonIgnore]
    public virtual ICollection<UserInfo> UserInfos { get; set; } = new List<UserInfo>();
}
