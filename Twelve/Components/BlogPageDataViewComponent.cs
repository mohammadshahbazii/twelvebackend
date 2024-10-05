using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class BlogPageDataViewComponent : ViewComponent
    {
        IBlogRepository blogRepository = new BlogRepository();
        public IViewComponentResult Invoke(string q="",int PageID=1)
        {
            var content = blogRepository.GetGroupPosts(q,PageID);
            ViewBag.q =q;
            return View("GetBlogPageData",content);
        }
    }
}
