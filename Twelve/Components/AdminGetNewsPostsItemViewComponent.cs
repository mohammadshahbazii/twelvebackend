using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class AdminGetNewsPostsItemViewComponent : ViewComponent
    {
        IBlogRepository blogRepository = new BlogRepository();
        public IViewComponentResult Invoke(string q = "", int PageID = 1)
        {
            var content = blogRepository.GetNews(q, PageID);
            ViewBag.PageID = PageID;

            return View("GetAdminGetNewsPostsItem", content);
        }
    }
}
