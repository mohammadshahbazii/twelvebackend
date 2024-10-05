using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class SearchFaqsViewComponent : ViewComponent
    {
        ISiteRepository siteRepository = new SiteRepository();
        public IViewComponentResult Invoke(string q="")
        {
            ViewBag.q = q;
            var content = siteRepository.GetFaqsBySearch(q);
            return View("GetSearchFaqs", content);
        }
    }
}
