using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class FaqGroup
{
    public int FaqGroupId { get; set; }

    public string GroupName { get; set; } = null!;

    public int? ParentId { get; set; }

    public virtual ICollection<SelectedFaqGroup> SelectedFaqGroups { get; set; } = new List<SelectedFaqGroup>();
}
