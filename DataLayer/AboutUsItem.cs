using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class AboutUsItem
{
    public int AboutUsItemId { get; set; }

    public string ItemTitle { get; set; } = null!;

    public string ItemDescription { get; set; } = null!;

    public string ImageName { get; set; } = null!;
}
