using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class AboutUsArticlesViewComponent : ViewComponent
    {
        ISiteRepository siteRepository = new SiteRepository();
        public IViewComponentResult Invoke()
        {
            var content = siteRepository.GetAboutUsArticles();
            return View("GetAboutUsArticles",content);
        }
    }
}
