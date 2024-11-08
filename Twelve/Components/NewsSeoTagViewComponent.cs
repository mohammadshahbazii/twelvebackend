using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class NewsSeoTagViewComponent : ViewComponent
    {
        IBlogRepository blogRepository = new BlogRepository();
        public IViewComponentResult Invoke(int blogId)
        {
            var content = blogRepository.GetNewsSeoTag(blogId);
            return View("GetNewsSeoTag", content);
        }
    }
}
