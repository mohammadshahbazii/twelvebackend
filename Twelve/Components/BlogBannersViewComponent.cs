using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class BlogBannersViewComponent : ViewComponent
    {
        IAdvertisementsRepository advertisementsRepository = new AdvertisementsRepository();
        public IViewComponentResult Invoke() 
        {
            var content = advertisementsRepository.GetBanners(); 
            return View("GetBlogBanners",content);
        }
    }
}
