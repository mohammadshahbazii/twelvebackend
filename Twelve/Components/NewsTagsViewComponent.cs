using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class NewsTagsViewComponent : ViewComponent
    {
        IBlogRepository blogRepository = new BlogRepository();
        public IViewComponentResult Invoke(int BlogID)
        {
            var content = blogRepository.GetNewsTags(BlogID);
            return View("GetNewsTags", content);
        }
    }
}
