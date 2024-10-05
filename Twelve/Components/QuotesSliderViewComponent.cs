using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class QuotesSliderViewComponent : ViewComponent
    {
        ISlidersRepository slidersRepository = new SlidersRepository();
        public IViewComponentResult Invoke()
        {
            var content = slidersRepository.GetQoutesSlider();
            return View("GetQuotesSlider", content);
        }
    }
}
