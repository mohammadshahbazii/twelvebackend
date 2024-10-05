using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class AdminblogAnswerCommentsItemViewComponent : ViewComponent
    {
        IBlogRepository blogRepository = new BlogRepository();
        public IViewComponentResult Invoke(string q = "", int PageID = 1)
        {
            var content = blogRepository.GetAnswerComments(q, PageID);


            return View("GetAdminblogAnswerCommentsItem", content);
        }
    }
}
