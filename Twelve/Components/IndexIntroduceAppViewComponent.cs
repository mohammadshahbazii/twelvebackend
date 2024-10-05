using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class IndexIntroduceAppViewComponent : ViewComponent
    {
        IFeaturesRepository featuresRepository = new FeaturesRepository();
        public IViewComponentResult Invoke()
        {
            var content = featuresRepository.GetIntroduceApp();
            return View("GetIndexIntroduceApp", content);
        }
    }
}
