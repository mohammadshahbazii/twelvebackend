using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class ContactUsContent
{
    public int ContactUsContentId { get; set; }

    public string FirstSubTitle { get; set; } = null!;

    public string FirstTitle { get; set; } = null!;

    public string FristDescription { get; set; } = null!;

    public string SecondSubTitle { get; set; } = null!;

    public string SecondTitle { get; set; } = null!;

    public string SecondDescription { get; set; } = null!;
}
