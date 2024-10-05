using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class NewsFeature
{
    public int NewsId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? ImageName { get; set; }

    public DateTime CreateDate { get; set; }

    public string ShortDescription { get; set; } = null!;

    public int PostView { get; set; }

    public string? Source { get; set; }

    public int? StudyTime { get; set; }

    public virtual ICollection<NewsComment> NewsComments { get; set; } = new List<NewsComment>();

    public virtual ICollection<NewsTag> NewsTags { get; set; } = new List<NewsTag>();

    public virtual ICollection<SelectedFeatureNews> SelectedFeatureNews { get; set; } = new List<SelectedFeatureNews>();
}
