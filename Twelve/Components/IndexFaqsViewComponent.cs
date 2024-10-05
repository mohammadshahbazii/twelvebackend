using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class IndexFaqsViewComponent : ViewComponent
    {
        IFaqsRepository faqsRepository = new FaqsRepository();
        public IViewComponentResult Invoke()
        {
            var content = faqsRepository.GetMainFaqs();
            return View("GetIndexFaqs", content);
        }
    }
}
