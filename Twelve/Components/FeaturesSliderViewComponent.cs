using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class FeaturesSliderViewComponent : ViewComponent
    {
        IFeaturesRepository featuresRepository = new FeaturesRepository();
        public IViewComponentResult Invoke()
        {
            var content = featuresRepository.GetFeatures();
            return View("GetFeaturesSlider", content);
        }
    }
}
