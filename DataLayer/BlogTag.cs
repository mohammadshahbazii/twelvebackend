using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class BlogTag
{
    public int BlogTagsId { get; set; }

    public string Tag { get; set; } = null!;

    public int BlogId { get; set; }

    public virtual Blog Blog { get; set; } = null!;
}
