using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class AdminFaqsItemViewComponent : ViewComponent
    {
        IFaqsRepository faqsRepository = new FaqsRepository();
        public IViewComponentResult Invoke(string q = "", int PageID = 1)
        {
            var content = faqsRepository.GetFaqs(q, PageID);


            return View("GetAdminFaqsItem", content);
        }
    }
}
