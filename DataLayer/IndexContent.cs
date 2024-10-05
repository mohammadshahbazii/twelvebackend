using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class IndexContent
{
    public int IndexContentId { get; set; }

    public string AboutTitle { get; set; } = null!;

    public string AboutSubTitle { get; set; } = null!;

    public string AboutDescription { get; set; } = null!;

    public string FeatureTitle { get; set; } = null!;

    public string FeatureSubTitle { get; set; } = null!;

    public string FeatureDescription { get; set; } = null!;

    public string FaqTitle { get; set; } = null!;

    public string FaqSubTitle { get; set; } = null!;

    public string FaqDescription { get; set; } = null!;
}
