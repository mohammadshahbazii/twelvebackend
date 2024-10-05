using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class IndexFeedBacksViewComponent : ViewComponent
    {
        IFeedBacksRepository feedBacksRepository = new FeedBacksRepository();
        public IViewComponentResult Invoke()
        {
            var content = feedBacksRepository.GetSelectedFeedBacks();
            return View("GetIndexFeedBacks", content);
        }
    }
}
