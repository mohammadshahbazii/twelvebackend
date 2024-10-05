using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class AdminNewsAnswerCommentsItemViewComponent : ViewComponent
    {
        IBlogRepository blogRepository = new BlogRepository();
        public IViewComponentResult Invoke(string q = "", int PageID = 1)
        {
            var content = blogRepository.GetNewsAnswerComments(q, PageID);


            return View("GetAdminNewsAnswerCommentsItem", content);
        }
    }
}
