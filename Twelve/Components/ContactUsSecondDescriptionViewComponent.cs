using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class ContactUsSecondDescriptionViewComponent : ViewComponent
    {
        ISiteRepository siteRepository = new SiteRepository();
        public IViewComponentResult Invoke()
        {
            var content = siteRepository.GetContactUsSecondDescription();
            return View("GetContactUsSecondDescription", content);
        }
    }
}
