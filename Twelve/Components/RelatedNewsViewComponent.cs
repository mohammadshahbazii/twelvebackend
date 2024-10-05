using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class RelatedNewsViewComponent : ViewComponent
    {
        IFeaturesRepository featuresRepository = new FeaturesRepository();
        public IViewComponentResult Invoke(int FeatureID)
        {
            var content = featuresRepository.GetRelatedNews(FeatureID);
            return View("GetRelatedNews", content);
        }
    }
}
