using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class AdminAboutUsItemsViewComponent : ViewComponent
    {
        IAboutUsItemsRepository aboutUsItemsRepository = new AboutUsItemsRepository();
        public IViewComponentResult Invoke()
        {
            var content = aboutUsItemsRepository.GetAboutUsItems();
            return View("GetAdminAboutUsItems",content);
        }
    }
}
