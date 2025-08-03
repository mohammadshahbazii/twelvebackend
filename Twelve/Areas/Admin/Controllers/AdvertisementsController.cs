using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using ViewModels;

namespace Twelve.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/Advertisements")]
    public class AdvertisementsController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();
        IAdvertisementsRepository advertisementsRepository = new AdvertisementsRepository();
        public IActionResult Index()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "لیست تبلیغات"))
            {
                var content = advertisementsRepository.GetAdvertisements();
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [Route("Create")]
        public IActionResult Create()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "لیست تبلیغات"))
            {
                return View(new AdvertisementCrudViewModel());
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(AdvertisementCrudViewModel advertisement, IFormFile imageProduct)
        {
            if (imageProduct == null)
            {
                ViewBag.Message = "لطفا تصویر مورد نظر را وارد کنید";
                return View(advertisement);
            }
            var entity = new Advertisement
            {
                Title = advertisement.Title,
                Link = advertisement.Link,
                IsBanner = advertisement.IsBanner
            };
            if (advertisementsRepository.Create(entity, imageProduct))
            {
                advertisement.AdvertisementId = entity.AdvertisementId;
                advertisementsRepository.SaveTranslations(advertisement);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                return View(advertisement);
            }
        }

        [Route("Update/{InfoID}")]
        public IActionResult Update(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "لیست تبلیغات"))
            {
                var content = advertisementsRepository.GetForEdit(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Update/{InfoID}")]
        public IActionResult Update(AdvertisementCrudViewModel advertisement, IFormFile imageProduct)
        {
            if (ModelState.IsValid)
            {
                var entity = new Advertisement
                {
                    AdvertisementId = advertisement.AdvertisementId,
                    Title = advertisement.Title,
                    Link = advertisement.Link,
                    IsBanner = advertisement.IsBanner,
                    ImageName = advertisement.ImageName
                };
                if (advertisementsRepository.Update(entity, imageProduct))
                {
                    advertisementsRepository.SaveTranslations(advertisement);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                    return View(advertisement);
                }
            }
            else
            {
                ViewBag.Message = "لطفا لینک مورد نظر را وارد کنید";
                var content = advertisementsRepository.GetForEdit(advertisement.AdvertisementId);
                return View(content);
            }
        }

        [Route("Delete/{InfoID}")]
        public IActionResult Delete(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "لیست تبلیغات"))
            {
                var content = advertisementsRepository.GetByID(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Delete/{InfoID}")]
        public IActionResult Delete(Advertisement Advertisements)
        {
            if (advertisementsRepository.Delete(Advertisements))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                return View();
            }
        }
    }
}
