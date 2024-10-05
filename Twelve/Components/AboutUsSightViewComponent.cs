using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class AboutUsSightViewComponent : ViewComponent
    {
        ISiteRepository siteRepository = new SiteRepository();
        public IViewComponentResult Invoke()
        {
            var content = siteRepository.GetAboutUsSights();
            return View("GetAboutUsSight",content);
        }
    }
}
