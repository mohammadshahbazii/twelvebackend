using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class Advertisement
{
    public int AdvertisementId { get; set; }

    public string? Title { get; set; }

    public string ImageName { get; set; } = null!;

    public string Link { get; set; } = null!;

    public bool IsBanner { get; set; }
}
