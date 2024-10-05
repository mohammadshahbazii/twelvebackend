using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/AboutUsItems")]
    public class AboutUsItemsController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();
        IAboutUsItemsRepository aboutUsItemsRepository = new AboutUsItemsRepository();
        public IActionResult Index()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "تنظیمات"))
            {
                var content = aboutUsItemsRepository.GetAboutUsItems();
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
            return View();
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(AboutUsItem item, IFormFile imageProduct)
        {
            if (aboutUsItemsRepository.Create(item, imageProduct))
            {
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                return View();
            }
        }

        [Route("Update/{ItemID}")]
        public IActionResult Update(int itemID)
        {
            var content = aboutUsItemsRepository.GetByID(itemID);
            return View(content);
        }

        [HttpPost]
        [Route("Update/{ItemID}")]
        public IActionResult Update(AboutUsItem item, IFormFile imageProduct)
        {
            if (aboutUsItemsRepository.Update(item, imageProduct))
            {
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = aboutUsItemsRepository.GetByID(item.AboutUsItemId);
                return View(content);
            }
        }

        [Route("Delete/{ItemID}")]
        public IActionResult Delete(int itemID)
        {
            var content = aboutUsItemsRepository.GetByID(itemID);
            return View(content);
        }

        [HttpPost]
        [Route("Delete/{ItemID}")]
        public IActionResult Delete(AboutUsItem item)
        {
            if (aboutUsItemsRepository.Delete(item))
            {
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = aboutUsItemsRepository.GetByID(item.AboutUsItemId);
                return View(content);
            }
        }
    }
}
