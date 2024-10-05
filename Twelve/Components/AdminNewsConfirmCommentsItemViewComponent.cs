using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class AdminNewsConfirmCommentsItemViewComponent : ViewComponent
    {
        IBlogRepository blogRepository = new BlogRepository();
        public IViewComponentResult Invoke(string q = "", int PageID = 1)
        {
            var content = blogRepository.GetNewsConfirmComments(q, PageID);


            return View("GetAdminNewsConfirmCommentsItem", content);
        }
    }
}
