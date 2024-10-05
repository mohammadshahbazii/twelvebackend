using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class AboutUsSliderViewComponent : ViewComponent
    {
        ISlidersRepository slidersRepository = new SlidersRepository();
        public IViewComponentResult Invoke()
        {
            var content = slidersRepository.GetAboutUsSlider();
            return View("GetAboutUsSlider", content);
        }
    }
}
