using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class CallInfoLink
{
    public int CallInfoLinkId { get; set; }

    public string Link { get; set; } = null!;

    public int CallInfoId { get; set; }

    public virtual CallInfo CallInfo { get; set; } = null!;
}
