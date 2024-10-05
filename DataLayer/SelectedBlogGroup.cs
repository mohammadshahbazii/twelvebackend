using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class SelectedBlogGroup
{
    public int SbgId { get; set; }

    public int BlogId { get; set; }

    public int BlogGroupId { get; set; }

    public virtual Blog Blog { get; set; } = null!;

    public virtual BlogGroup BlogGroup { get; set; } = null!;
}
