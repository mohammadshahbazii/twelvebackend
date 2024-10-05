using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class IndexBlogsViewComponent : ViewComponent
    {
        IBlogRepository blogRepository = new BlogRepository();
        public IViewComponentResult Invoke()
        {
            var content = blogRepository.GetIndexBlogs();
            return View("GetIndexBlogs", content);
        }
    }
}
