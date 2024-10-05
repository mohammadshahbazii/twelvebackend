using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class AdvertisementsViewComponent : ViewComponent
    {
        IAdvertisementsRepository advertisementsRepository = new AdvertisementsRepository();
        public IViewComponentResult Invoke()
        {
            var content = advertisementsRepository.GetLittleAds();
            return View("GetAdvertisements", content);
        }
    }
}
