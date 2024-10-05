using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class RelatedBlogsViewComponent : ViewComponent
    {
        IBlogRepository blogRepository = new BlogRepository();
        public IViewComponentResult Invoke(int BlogID) 
        { 
            var content = blogRepository.GetRelatedPosts(BlogID);
            return View("GetRelatedBlogs",content);
        }
    }
}
