using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class CallInfo
{
    public int CallInfoId { get; set; }

    public string Title { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public string? ImageName { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<CallInfoLink> CallInfoLinks { get; set; } = new List<CallInfoLink>();
}
