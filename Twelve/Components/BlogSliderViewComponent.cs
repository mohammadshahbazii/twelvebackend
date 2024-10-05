using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class BlogSliderViewComponent : ViewComponent
    {
        ISlidersRepository slidersRepository = new SlidersRepository();
        public IViewComponentResult Invoke()
        {
            var content = slidersRepository.GetBlogSlider();
            return View("GetBlogSlider", content);
        }
    }
}
