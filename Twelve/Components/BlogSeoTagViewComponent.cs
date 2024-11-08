using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class BlogSeoTagViewComponent : ViewComponent
    {
        IBlogRepository blogRepository = new BlogRepository();
        public IViewComponentResult Invoke(int blogId)
        {
            var content = blogRepository.GetBlogSeoTag(blogId);
            return View("GetBlogSeoTag", content);
        }
    }
}
