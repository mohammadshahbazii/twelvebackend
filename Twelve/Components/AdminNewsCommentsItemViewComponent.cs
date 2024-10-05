using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class AdminNewsCommentsItemViewComponent : ViewComponent
    {
        IBlogRepository blogRepository = new BlogRepository();
        public IViewComponentResult Invoke(string q = "", int PageID = 1)
        {
            var content = blogRepository.GetNewsComments(q, PageID);


            return View("GetAdminNewsCommentsItem", content);
        }
    }
}
