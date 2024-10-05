using DataLayer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Services
{
    public interface IBlogRepository : IDisposable
    {
        public string GetBlogBanner();
        public CreateRelatedBlogsViewModel GetModelForCreateRelatedBlog(int BlogID , string q ="");

        public bool CreateRelatedBlogs(CreateRelatedBlogsViewModel blogs);

        public List<BlogItemViewModel> GetGroupsForRelated(string q =""); 
        public bool AnswerComment(BlogComment comment);
        public bool UpdateComment(BlogComment comment);
        public bool Delete(BlogComment comment);
        public BlogComment GetAnswerByID(int id);
        public BlogComment GetCommentByID(int id);

        #region NewsCrud
        public bool AnswerComment(NewsComment comment);
        public bool UpdateComment(NewsComment comment);
        public bool Delete(NewsComment comment);
        public NewsComment GetNewsAnswerByID(int id);
        public NewsComment GetNewsCommentByID(int id);
        public bool CreateNews(BlogCrudViewModel blog, IFormFile ImageName);
        public bool UpdateNews(BlogCrudViewModel blog, IFormFile ImageName);
        public bool DeleteNews(int ID);
        public BlogCrudViewModel GetNewsModelForCreate();
        public BlogCrudViewModel GetNewsByID(int BlogID);

        #endregion NewsCrud

        public bool Create(BlogCrudViewModel blog, IFormFile ImageName);
        public bool Update(BlogCrudViewModel blog, IFormFile ImageName);
        public bool Delete(int ID);
        public BlogCrudViewModel GetModelForCreate();
        public BlogCrudViewModel GetByID(int BlogID);
        public BlogCommentsPageDataViewModel GetNewsComments(string q = "", int PageID = 1);
        public BlogCommentsPageDataViewModel GetNewsConfirmComments(string q = "", int PageID = 1);
        public BlogCommentsPageDataViewModel GetNewsAnswerComments(string q = "", int PageID = 1);

        public BlogCommentsPageDataViewModel GetComments(string q = "", int PageID = 1);
        public BlogCommentsPageDataViewModel GetConfirmComments(string q = "", int PageID = 1);
        public BlogCommentsPageDataViewModel GetAnswerComments(string q = "", int PageID = 1);

        public AdminBlogPageDataViewModel GetBlogs(string q = "", int PageID = 1);
        public AdminBlogPageDataViewModel GetNews(string q = "", int PageID = 1);

        public BlogGroupsPostsPaginationViewModel GetPostsByPagination(int GroupID , int PageID=1 , string q = "");
        public BlogGroupsPostsPaginationViewModel GetNewsByPagination( int PageID);

        public PaginationViewModel GetBlogsPagination(double PageCount, int PageID = 1);

        public BlogPagesDataViewModel GetGroupPosts(string q = "", int PageID = 1);

        public List<BlogItemViewModel> GetRelatedPosts(int BlogID);
        public string SubmitComment(int BlogID , string fullname , string title , string text);
        public string SubmitNewsComment(int BlogID , string fullname , string title , string text);
        public BlogPostCommentsViewModel GetBlogComments(int BlogID , int PageID = 1);
        public List<NewsComment> GetNewsComments(int BlogID);

        public List<BlogItemViewModel> LatestBlogs();
        public List<IndexSliderItemViewModel> PopularBlogs();

        public List<BlogItemViewModel> LatestNews();
        public List<IndexSliderItemViewModel> PopularNews();

        public List<string> GetBlogTags(int BlogID);
        public List<string> GetNewsTags(int BlogID);
        public BlogPostPageDataViewModel GetBlogPageData(int BlogID);
        public BlogPostPageDataViewModel GetNewsPageData(int BlogID);
        public IndexBlogsViewModel GetIndexBlogs();
    }
}
