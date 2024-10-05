using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class FaqContentViewComponent : ViewComponent
    {
        IFaqsRepository faqsRepository = new FaqsRepository();
        public IViewComponentResult Invoke()
        {
            var content = faqsRepository.GetFaqContent();
            return View("GetFaqContent",content);
        }
    }
}
