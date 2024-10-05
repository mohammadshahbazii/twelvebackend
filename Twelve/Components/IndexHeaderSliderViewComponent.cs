using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class IndexHeaderSliderViewComponent : ViewComponent
    {
        ISlidersRepository slidersRepository = new SlidersRepository();
        public IViewComponentResult Invoke()
        {
            var content = slidersRepository.GetIndexHeader();
            return View("GetIndexHeaderSlider",content);
        }
    }
}
