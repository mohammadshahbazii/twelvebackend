using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Controllers
{
    public class FeatureController : Controller
    {
        IFeaturesRepository featuresRepository = new FeaturesRepository();

        [Route("Feature/{Demo}")]
        public IActionResult Index(string Demo)
        {
            var content = featuresRepository.GetFeature(Demo);
            return View(content);
        }

        [Route("AppMenu")]
        public IActionResult AppMenu()
        {
            return View();
        }
    }
}
