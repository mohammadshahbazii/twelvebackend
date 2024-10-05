using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class BlogComment
{
    public int CommentId { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Email { get; set; }

    public string Text { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public int? ParentId { get; set; }

    public int BlogId { get; set; }

    public string Title { get; set; } = null!;

    public bool IsConfirm { get; set; }

    public bool IsAnswer { get; set; }

    public virtual Blog Blog { get; set; } = null!;
}
