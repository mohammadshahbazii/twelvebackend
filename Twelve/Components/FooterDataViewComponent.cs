using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class FooterDataViewComponent : ViewComponent
    {
        ISiteRepository siteRepository = new SiteRepository();
        public IViewComponentResult Invoke()
        {
            var content = siteRepository.GetDownloadBox();
            return View("GetFooterData", content);
        }
    }
}
