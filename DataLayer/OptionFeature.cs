using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class OptionFeature
{
    public int OptionId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int FeatureId { get; set; }

    public string ImageName { get; set; } = null!;

    public virtual Feature Feature { get; set; } = null!;
}
