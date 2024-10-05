using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class SliderGroup
{
    public int SliderGroupId { get; set; }

    public string GroupName { get; set; } = null!;

    public virtual ICollection<Slider> Sliders { get; set; } = new List<Slider>();
}
