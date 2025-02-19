using System;
using System.Collections.Generic;

namespace Galini.Models.Entity;

public partial class RefreshToken
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime ExpirationTime { get; set; }

    public virtual Account User { get; set; } = null!;
}
