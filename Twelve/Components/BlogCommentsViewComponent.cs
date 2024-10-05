using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class BlogCommentsViewComponent : ViewComponent
    {
        IBlogRepository blogRepository = new BlogRepository();
        public IViewComponentResult Invoke(int BlogID , int PageID = 1) 
        {
            var content = blogRepository.GetBlogComments(BlogID,PageID);
            return View("GetBlogComments",content); 
        }
    }
}
