using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class NewsCommentsViewComponent : ViewComponent
    {
        IBlogRepository blogRepository = new BlogRepository();
        public IViewComponentResult Invoke(int BlogID)
        {
            var content = blogRepository.GetNewsComments(BlogID);
            return View("GetNewsComments", content);
        }
    }
}
