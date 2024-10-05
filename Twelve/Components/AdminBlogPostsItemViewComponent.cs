using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class AdminBlogPostsItemViewComponent : ViewComponent
    {
        IBlogRepository blogRepository = new BlogRepository();
        public IViewComponentResult Invoke(string q = "", int PageID = 1)
        {
            var content = blogRepository.GetBlogs(q, PageID);
            ViewBag.PageID = PageID;

            return View("GetAdminBlogPostsItem", content);
        }
    }
}
