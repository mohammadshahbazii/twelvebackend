using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class SelectedFaqGroup
{
    public int SfgId { get; set; }

    public int FaqGroupId { get; set; }

    public int FaqId { get; set; }

    public virtual Faq Faq { get; set; } = null!;

    public virtual FaqGroup FaqGroup { get; set; } = null!;
}
