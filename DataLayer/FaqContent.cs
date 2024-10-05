using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class FaqContent
{
    public int FaqContentId { get; set; }

    public string FaqContentTitle { get; set; } = null!;

    public string FaqContentSubTitle { get; set; } = null!;

    public string FaqContentDescription { get; set; } = null!;
}
