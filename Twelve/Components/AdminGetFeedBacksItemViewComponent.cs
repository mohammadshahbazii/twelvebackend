using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class AdminGetFeedBacksItemViewComponent : ViewComponent
    {
        IFeedBacksRepository feedBacksRepository = new FeedBacksRepository();
        public IViewComponentResult Invoke(string q = "", int PageID = 1)
        {
            var content = feedBacksRepository.GetFeedBacks(q, PageID);


            return View("GetAdminGetFeedBacksItem", content);
        }
    }
}
