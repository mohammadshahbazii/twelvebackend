using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class Blog
{
    public int BlogId { get; set; }

    public string Title { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int PostView { get; set; }

    public DateTime CreateDate { get; set; }

    public string ImageName { get; set; } = null!;

    public string? Source { get; set; }

    public bool IsSlider { get; set; }

    public int StudyTime { get; set; }

    public virtual ICollection<BlogComment> BlogComments { get; set; } = new List<BlogComment>();

    public virtual ICollection<BlogTag> BlogTags { get; set; } = new List<BlogTag>();

    public virtual ICollection<SelectedBlogGroup> SelectedBlogGroups { get; set; } = new List<SelectedBlogGroup>();

    public virtual ICollection<SelectedRelatedBlog> SelectedRelatedBlogMainBlogs { get; set; } = new List<SelectedRelatedBlog>();

    public virtual ICollection<SelectedRelatedBlog> SelectedRelatedBlogRelatedBlogs { get; set; } = new List<SelectedRelatedBlog>();
}
