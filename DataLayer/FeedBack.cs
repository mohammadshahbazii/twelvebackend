using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class FeedBack
{
    public int FeedBackId { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Email { get; set; }

    public string Subject { get; set; } = null!;

    public string Text { get; set; } = null!;

    public bool IsShow { get; set; }

    public string? ImageName { get; set; }

    public DateTime CreateDate { get; set; }
}
