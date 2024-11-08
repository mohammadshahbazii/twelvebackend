using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class MainSeoTagsViewComponent : ViewComponent
    {
        ISiteRepository siteRepository = new SiteRepository();
        public IViewComponentResult Invoke()
        {
            var content = siteRepository.GetSeoTags();
            return View("GetMainSeoTags",content);
        }

    }
}
