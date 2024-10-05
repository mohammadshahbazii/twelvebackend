using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class FeaturesContent
{
    public int FeatureContentId { get; set; }

    public string FeaturesTitle { get; set; } = null!;

    public string FeatruesSubTitle { get; set; } = null!;

    public string FeaturesDescription { get; set; } = null!;
}
