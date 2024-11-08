using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Controllers
{
    public class BlogController : Controller
    {
        IBlogRepository blogRepository =new BlogRepository();

        public IActionResult Index()
        {
            ViewBag.Banner = blogRepository.GetBlogBanner();
            return View();
        }

        public IActionResult SearchBlogs(string q = "")
        {
            return ViewComponent("BlogPageData", new { q = q });
        }

        public IActionResult GetBlogs(int GroupID , int PageID = 1, string q = "")
        {
            return ViewComponent("PostsByPagination",new { GroupID = GroupID , PageID = PageID,q=q });
        }

        public IActionResult GetBlogComments(int BlogID, int PageID = 1)
        {
            return ViewComponent("BlogCommentsPagination", new { BlogID = BlogID, PageID = PageID });
        }

        public IActionResult GetNews(int PageID)
        {
            return ViewComponent("NewsByPagination", new { PageID = PageID });
        }

        [Route("BlogsByTag/{tag}")]
        public IActionResult BlogsByTag(string tag)
        {
            var content = blogRepository.GetBlogsByTag(tag);
            ViewBag.tag = tag;
            return View(content);
        }

        [Route("PostBlogs/{BlogID}")]
        public IActionResult PostBlog(int BlogID)
        {
            var content = blogRepository.GetBlogPageData(BlogID);
            return View(content);
        }

        [Route("PostNews/{BlogID}")]
        public IActionResult PostNews(int BlogID)
        {
            var content = blogRepository.GetNewsPageData(BlogID);
            return View(content);
        }

        [Route("SubmitComment")]
        public string SubmitComment(int BlogID,string fullname, string title, string Text)
        {
            IBlogRepository blogRepository = new BlogRepository();
            return blogRepository.SubmitComment(BlogID,fullname, title, Text);
        }

        [Route("SubmitNewsComment")]
        public string SubmitNewsComment(int BlogID, string fullname, string title, string Text)
        {
            IBlogRepository blogRepository = new BlogRepository();
            return blogRepository.SubmitNewsComment(BlogID, fullname, title, Text);
        }
    }
}
