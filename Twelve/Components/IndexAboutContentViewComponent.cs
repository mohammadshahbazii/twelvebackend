using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class IndexAboutContentViewComponent : ViewComponent
    {
        ISiteRepository siteRepository = new SiteRepository();
        public IViewComponentResult Invoke()
        {
            var content = siteRepository.GetIndexAbout();
            return View("GetIndexAboutContent", content);
        }
    }
}
