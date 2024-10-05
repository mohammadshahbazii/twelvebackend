using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class NewsByPaginationViewComponent : ViewComponent
    {
        IBlogRepository blogRepository = new BlogRepository();
        public IViewComponentResult Invoke(int PageID)
        {
            var content = blogRepository.GetNewsByPagination( PageID);
            return View("GetNewsByPagination", content);
        }
    }
}
