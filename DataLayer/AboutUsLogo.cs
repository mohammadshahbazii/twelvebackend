using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class AboutUsLogo
{
    public int AboutUsLogoId { get; set; }

    public string ImageName { get; set; } = null!;

    public bool IsMain { get; set; }
}
