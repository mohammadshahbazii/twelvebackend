using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class AdminBlogRelatedPostsViewComponent : ViewComponent
    {
        IBlogRepository blogRepository = new BlogRepository();
        public IViewComponentResult Invoke(int BlogID,string q = "")
        {
            var content = blogRepository.GetModelForCreateRelatedBlog(BlogID,q);

            return View("GetAdminBlogRelatedPosts", content);
        }
    }
}
