using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class IntroduceSlider
{
    public int IntroduceSliderId { get; set; }

    public int FeatureId { get; set; }

    public string Title { get; set; } = null!;

    public string ImageName { get; set; } = null!;

    public virtual Feature Feature { get; set; } = null!;
}
