using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Repository;

namespace Twelve.Components
{
    public class AdminAboutUsLogoesViewComponent : ViewComponent
    {
        IAboutUsLogoesRepository aboutUsLogoesRepository =new AboutUsLogoesRepository();
        public IViewComponentResult Invoke()
        {
            var content= aboutUsLogoesRepository.GetAboutUsLogos();
            return View("GetAdminAboutUsLogoes",content);
        }
    }
}
