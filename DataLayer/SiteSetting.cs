using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class SiteSetting
{
    public int SiteSettingId { get; set; }

    public string Title { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string DirectDownloadLink { get; set; } = null!;

    public int DownloadCount { get; set; }

    public string? BlogBannerImage { get; set; }
}
