using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class AboutUsSightsViewComponent : ViewComponent
    {
        IAboutUsSightRepository aboutUsSightRepository = new AboutUsSightRepository();
        public IViewComponentResult Invoke()
        {
            var content = aboutUsSightRepository.GetAboutUsSights();
            return View("GetAboutUsSights",content);
        }
    }
}
