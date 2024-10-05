using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class PostsByPaginationViewComponent : ViewComponent
    {
        IBlogRepository blogRepository = new BlogRepository();
        public IViewComponentResult Invoke(int GroupID , int PageID=1 , string q = "")
        {
            var content = blogRepository.GetPostsByPagination(GroupID, PageID,q);
            return View("GetPostsByPagination",content);
        }
    }
}
