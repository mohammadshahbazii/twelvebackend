using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class AdminblogConfirmCommentsItemViewComponent : ViewComponent
    {
        IBlogRepository blogRepository = new BlogRepository();
        public IViewComponentResult Invoke(string q = "", int PageID = 1)
        {
            var content = blogRepository.GetConfirmComments(q, PageID);


            return View("GetAdminblogConfirmCommentsItem", content);
        }
    }
}
