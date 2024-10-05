using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class LatestNewsViewComponent : ViewComponent
    {
        IBlogRepository blogRepository = new BlogRepository();
        public IViewComponentResult Invoke()
        {
            var content = blogRepository.LatestNews();
            return View("GetLatestNews", content);
        }
    }
}
