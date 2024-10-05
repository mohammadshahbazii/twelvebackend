using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class BlogTagsViewComponent : ViewComponent
    {
        IBlogRepository blogRepository = new BlogRepository();
        public IViewComponentResult Invoke(int BlogID)
        {
            var content = blogRepository.GetBlogTags(BlogID);
            return View("GetBlogTags", content);
        }
    }
}
