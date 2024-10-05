using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class AboutUsArticle
{
    public int AboutUsArticleId { get; set; }

    public string SubTitle { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ImageName { get; set; } = null!;
}
