using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class NewsTag
{
    public int FntId { get; set; }

    public string Tag { get; set; } = null!;

    public int NewsId { get; set; }

    public virtual NewsFeature News { get; set; } = null!;
}
