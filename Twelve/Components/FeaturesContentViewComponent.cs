using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class FeaturesContentViewComponent : ViewComponent
    {
        IFeaturesRepository featuresRepository = new FeaturesRepository();
        public IViewComponentResult Invoke()
        {
            var content = featuresRepository.GetFeaturesContent();
            return View("GetFeaturesContent",content);
        }
    }
}
