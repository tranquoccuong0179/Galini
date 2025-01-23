using System;
using System.Collections.Generic;

namespace Galini.Models.Entity;

public partial class Identification
{
    public int Id { get; set; }

    public string IdentificationCode { get; set; } = null!;

    public int Duration { get; set; }
}
