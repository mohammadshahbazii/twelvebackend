using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class NewsComment
{
    public int NewsCommentId { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Email { get; set; }

    public string Text { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public int? ParentId { get; set; }

    public int NewsId { get; set; }

    public bool IsConfirm { get; set; }

    public bool IsAnswer { get; set; }

    public string Title { get; set; } = null!;

    public virtual NewsFeature News { get; set; } = null!;
}
