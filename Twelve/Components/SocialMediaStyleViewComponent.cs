using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class SocialMediaStyleViewComponent : ViewComponent
    {
        ISiteRepository siteRepository = new SiteRepository();
        public IViewComponentResult Invoke()
        {
            var content = siteRepository.GetSocialMedia();
            return View("GetSocialMediaStyle",content);
        }
    }
}
