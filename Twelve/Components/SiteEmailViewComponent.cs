using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class SiteEmailViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            ISiteRepository siteRepository = new SiteRepository();
            ViewBag.Email = siteRepository.GetSiteSetting().Email;
            return View("GetSiteEmail");
        }
    }
}
