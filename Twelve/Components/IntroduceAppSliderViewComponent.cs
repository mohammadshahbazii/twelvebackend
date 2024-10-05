using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class IntroduceAppSliderViewComponent : ViewComponent
    {
        IIntroduceSlidersRepository introduceSlidersRepository = new IntroduceSlidersRepository();
        public IViewComponentResult Invoke(int FeatureID)
        {
            var content = introduceSlidersRepository.GetListByFeatureID(FeatureID);
            return View("GetIntroduceAppSlider",content);
        }
    }
}
