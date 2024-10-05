using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class SelectedRelatedBlog
{
    public int SrbId { get; set; }

    public int MainBlogId { get; set; }

    public int RelatedBlogId { get; set; }

    public int? FeatureId { get; set; }

    public virtual Blog MainBlog { get; set; } = null!;

    public virtual Blog RelatedBlog { get; set; } = null!;
}
