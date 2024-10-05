using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class IndexFeaturesViewComponent : ViewComponent
    {
        IFeaturesRepository featuresRepository = new FeaturesRepository();
        public IViewComponentResult Invoke()
        {
            var content = featuresRepository.GetFeatures();
            return View("GetIndexFeatures", content);
        }
    }
}
