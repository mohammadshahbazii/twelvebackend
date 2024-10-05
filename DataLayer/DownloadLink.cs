using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class DownloadLink
{
    public int DownloadLinkId { get; set; }

    public string Title { get; set; } = null!;

    public string? ShortDescription { get; set; }

    public string ImageName { get; set; } = null!;

    public string Link { get; set; } = null!;

    public int DlgroupId { get; set; }

    public virtual DownloadLinkGroup Dlgroup { get; set; } = null!;
}
