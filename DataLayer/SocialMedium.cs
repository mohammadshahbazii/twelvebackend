using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class SocialMedium
{
    public int SocialMediaId { get; set; }

    public string Title { get; set; } = null!;

    public string ImageName { get; set; } = null!;

    public string Link { get; set; } = null!;

    public string? ClassName { get; set; }

    public string? ColorCode { get; set; }
}
