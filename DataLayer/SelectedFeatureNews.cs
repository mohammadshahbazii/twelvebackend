using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class SelectedFeatureNews
{
    public int SfnId { get; set; }

    public int NewsId { get; set; }

    public int FeatureId { get; set; }

    public virtual Feature Feature { get; set; } = null!;

    public virtual NewsFeature News { get; set; } = null!;
}
