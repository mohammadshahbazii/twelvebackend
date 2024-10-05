using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class LatestBlogsViewComponent : ViewComponent
    {
        IBlogRepository blogRepository = new BlogRepository();
        public IViewComponentResult Invoke()
        {
            var content = blogRepository.LatestBlogs();
            return View("GetLatestBlogs", content);
        }
    }
}
