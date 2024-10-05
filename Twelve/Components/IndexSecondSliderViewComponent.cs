using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class IndexSecondSliderViewComponent : ViewComponent
    {
        ISlidersRepository slidersRepository = new SlidersRepository();
        public IViewComponentResult Invoke()
        {
            var content = slidersRepository.GetIndexSecondSlider();
            return View("GetIndexSecondSlider", content);
        }
    }
}
