using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class FeatureItem
{
    public int FeaturesItemId { get; set; }

    public string Title { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public string ImageName { get; set; } = null!;

    public int FeatureId { get; set; }

    public virtual Feature Feature { get; set; } = null!;
}
