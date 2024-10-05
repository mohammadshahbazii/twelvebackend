using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class AboutUsLogoesViewComponent : ViewComponent
    {
        ISiteRepository siteRepository = new SiteRepository();
        public IViewComponentResult Invoke()
        {
            var content = siteRepository.GetAboutUsLogoImages();
            return View("GetAboutUsLogoes", content);
        }
    }
}
