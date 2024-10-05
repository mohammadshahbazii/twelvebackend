using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class ContactUsFirstDescriptionViewComponent : ViewComponent
    {
        ISiteRepository siteRepository = new SiteRepository();
        public IViewComponentResult Invoke() 
        {
            var content = siteRepository.GetContactUsFirstDescription();
            return View("GetContactUsFirstDescription",content);
        }
    }
}
