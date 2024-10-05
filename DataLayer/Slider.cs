using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class Slider
{
    public int SliderId { get; set; }

    public string ImageName { get; set; } = null!;

    public string Title { get; set; } = null!;

    public int SliderGroupId { get; set; }

    public string? ShortDescription { get; set; }

    public string? Link { get; set; }

    public virtual SliderGroup SliderGroup { get; set; } = null!;
}
