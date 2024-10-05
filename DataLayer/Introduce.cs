using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class Introduce
{
    public int IntroduceId { get; set; }

    public string IntroduceText { get; set; } = null!;

    public int FeatureId { get; set; }

    public string IntroduceTitle { get; set; } = null!;

    public virtual Feature Feature { get; set; } = null!;
}
