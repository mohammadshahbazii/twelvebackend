using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class AboutUsSight
{
    public int AboutUsSightId { get; set; }

    public string SightTitle { get; set; } = null!;

    public string SightDescription { get; set; } = null!;

    public string ImageName { get; set; } = null!;
}
