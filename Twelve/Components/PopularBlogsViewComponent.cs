using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class PopularBlogsViewComponent : ViewComponent
    {
        IBlogRepository blogRepository = new BlogRepository();
        public IViewComponentResult Invoke()
        {
            var content = blogRepository.PopularBlogs();
            return View("GetPopularBlogs", content);
        }
    }
}
