using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class FaqTranslation
{
    public int FaqTranslationId { get; set; }
    public int FaqId { get; set; }
    public string Language { get; set; } = null!;
    public string Question { get; set; } = null!;
    public string Answer { get; set; } = null!;

    public virtual Faq Faq { get; set; } = null!;
}
