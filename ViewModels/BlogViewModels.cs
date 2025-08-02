using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class BlogPostCommentsViewModel
    {
        public List<BlogCommentViewModel> Comments { get; set; }
        public PaginationViewModel Pagination { get; set; }
        public double PageCount { get; set; }
        public int CommentCount { get; set; }
        public int BlogID { get; set; }
    }

    public class BlogCrudViewModel
    {
        public int BlogID { get; set; }
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }
        [Display(Name = "توضیح کوتاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ShortDescription { get; set; }
        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }
        public string TitleEn { get; set; }
        public string TitleAr { get; set; }
        public string TitleUr { get; set; }
        public string ShortDescriptionEn { get; set; }
        public string ShortDescriptionAr { get; set; }
        public string ShortDescriptionUr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionUr { get; set; }
        public string ImageName { get; set; }
        public string Source { get; set; }
        public string Tags { get; set; }
        public bool IsSlider { get; set; }
        public int StudyTime { get; set; }
        public List<BlogGroupNameViewModel> Groups { get; set; }
        public List<int> SelectedGroups { get; set; }
    }

    public class BlogCommentViewModel
    {
        public int CommentID { get; set; }
        public string Fullname { get; set; }
        public string CommentText { get; set; }
        public string CreateDate { get; set; }
        public int BlogID { get; set; }
        public int? ParentID { get; set; }
        public string BlogTitle { get; set; }
        public bool IsAnswer { get; set; }
        public bool IsConfirm { get; set; }
    }
    public class BlogCommentsPageDataViewModel
    {
        public List<BlogCommentViewModel> Comments { get; set; }
        public PaginationViewModel Pagination { get; set; }
        public double PageCount { get; set; }
    }
    public class BlogGroupNameViewModel
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public int? ParentID { get; set; }

        public int Count { get; set; }
    }
    public class BlogPostsItemViewModel
    {
        public int BlogID { get; set; }
        public string Title { get; set; }
        public int View { get; set; }
        public string GroupName { get; set; }
        public string CreateDate { get; set; }
    }

    public class AdminBlogPageDataViewModel
    {
        public List<BlogPostsItemViewModel> Posts { get; set; }
        public PaginationViewModel Pagination { get; set; }
        public double PageCount { get; set; }

    }
    public class BlogGroupsItemViewModel
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
    }

    public class CreateRelatedBlogsViewModel
    {
        public int BlogID { get; set; }
        public List<BlogItemViewModel> Blogs { get; set; }
        public List<int> SelectedBlogs { get; set; }
    }

    public class BlogPostPageDataViewModel
    {
        public int BlogID { get; set; }
        public string GroupName { get; set; }
        public int GroupID { get; set; }
        public string CreateDate { get; set; }
        public string StudyTime { get; set; }
        public string ImageName { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
    }

    public class BlogItemViewModel
    {
        public int BlogID { get; set; }
        public int GroupID { get; set; }
        public string Title { get; set; }
        public string CreateDate { get; set; }
        public string ImageName { get; set; }
        public string GroupName { get; set; }
        public string Source { get; set; }

        public bool IsBlog { get; set; }
    }

    public class BlogGroupsPostsViewModel
    {
        public int GroupID { get; set; }
        public List<BlogItemViewModel> Posts { get; set; }
    }

    public class BlogGroupsPostsPaginationViewModel
    {
        public int GroupID { get; set; }
        public List<BlogItemViewModel> Posts { get; set; }
        public PaginationViewModel Pagination { get; set; }
    }

    public class BlogPagesDataViewModel
    {
        public List<BlogGroupsItemViewModel> Groups { get; set; }
        public List<BlogGroupsPostsPaginationViewModel> Blogs { get; set; }
    }

    public class IndexBlogsViewModel
    {
        public List<BlogItemViewModel> Blogs { get; set; }
        public List<BlogItemViewModel> News { get; set; }
    }
}
