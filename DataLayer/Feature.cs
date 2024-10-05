using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class Feature
{
    public int FeatureId { get; set; }

    public string Title { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public string ImageName { get; set; } = null!;

    public string FirstDescription { get; set; } = null!;

    public string? SecondDescription { get; set; }

    public string? FirstArticleTitle { get; set; }

    public string? FirstArticleDescription { get; set; }

    public string? SecondArticleTitle { get; set; }

    public string? SecondArticleDescription { get; set; }

    public string? FirstArticleImage { get; set; }

    public string? SecondArticleImage { get; set; }

    public string AnimateFilename { get; set; } = null!;

    public string? IntroduceImageName { get; set; }

    public virtual ICollection<FeatureItem> FeatureItems { get; set; } = new List<FeatureItem>();

    public virtual ICollection<IntroduceSlider> IntroduceSliders { get; set; } = new List<IntroduceSlider>();

    public virtual ICollection<Introduce> Introduces { get; set; } = new List<Introduce>();

    public virtual ICollection<OptionFeature> OptionFeatures { get; set; } = new List<OptionFeature>();

    public virtual ICollection<SelectedFeatureNews> SelectedFeatureNews { get; set; } = new List<SelectedFeatureNews>();
}
