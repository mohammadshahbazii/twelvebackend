using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class BlogTranslation
{
    public int BlogTranslationId { get; set; }

    public int BlogId { get; set; }

    public string Language { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual Blog Blog { get; set; } = null!;
}
