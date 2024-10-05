using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class PopularNewsViewComponent : ViewComponent
    {
        IBlogRepository blogRepository = new BlogRepository();
        public IViewComponentResult Invoke()
        {
            var content = blogRepository.PopularNews();
            return View("GetPopularNews", content);
        }
    }
}
