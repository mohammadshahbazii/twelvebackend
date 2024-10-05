using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class AdminIntroduceFeaturesItemViewComponent : ViewComponent
    {
        IFeaturesRepository featuresRepository = new FeaturesRepository();
        public IViewComponentResult Invoke(string q = "", int PageID = 1)
        {
            var content = featuresRepository.GetAdminFeatures(q, PageID);


            return View("GetAdminIntroduceFeaturesItem", content);
        }
    }
}
