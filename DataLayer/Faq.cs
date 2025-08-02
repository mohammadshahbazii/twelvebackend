using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class Faq
{
    public int FaqId { get; set; }

    public string Question { get; set; } = null!;

    public string Answer { get; set; } = null!;

    public bool IsMain { get; set; }

    public virtual ICollection<SelectedFaqGroup> SelectedFaqGroups { get; set; } = new List<SelectedFaqGroup>();

    public virtual ICollection<FaqTranslation> FaqTranslations { get; set; } = new List<FaqTranslation>();
}
