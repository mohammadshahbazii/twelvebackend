using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Controllers
{
    public class SearchController : Controller
    {
        ISiteRepository siteRepository = new SiteRepository();
        public IActionResult Index(string q="")
        {
            ViewBag.q = q;
            var content =  siteRepository.GetSearchContent(q);
            return View(content);
        }

        [Route("SearchFaqs")]
        public IActionResult SearchFaqs(string q = "")
        {
            return ViewComponent("SearchFaqs",new { q= q });
            
        }
    }
}
