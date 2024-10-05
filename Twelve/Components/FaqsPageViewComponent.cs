using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class FaqsPageViewComponent : ViewComponent
    {
        IFaqsRepository faqsRepository = new FaqsRepository();
        public IViewComponentResult Invoke()
        {
            var content = faqsRepository.GetFaqGroups();
            return View("GetFaqsPage", content);
        }
    }
}
