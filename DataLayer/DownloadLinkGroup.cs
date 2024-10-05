using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class DownloadLinkGroup
{
    public int DlgroupId { get; set; }

    public string GroupName { get; set; } = null!;

    public virtual ICollection<DownloadLink> DownloadLinks { get; set; } = new List<DownloadLink>();
}
