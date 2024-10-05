using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class AdminAboutUsArticlesViewComponent : ViewComponent
    {
        IAboutUsArticlesRepository aboutUsArticlesRepository = new AboutUsArticlesRepository();
        public IViewComponentResult Invoke()
        {
            var content = aboutUsArticlesRepository.GetAboutUsArticles();
            return View("GetAdminAboutUsArticles",content);
        }
    }
}
