using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class BlogGroup
{
    public int BlogGroupId { get; set; }

    public string GroupName { get; set; } = null!;

    public int? ParentId { get; set; }

    public virtual ICollection<SelectedBlogGroup> SelectedBlogGroups { get; set; } = new List<SelectedBlogGroup>();
}
