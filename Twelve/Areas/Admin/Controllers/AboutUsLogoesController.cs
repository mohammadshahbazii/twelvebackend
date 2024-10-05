using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Repository;

namespace Twelve.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/AboutUsLogoes")]
    public class AboutUsLogoesController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();
        IAboutUsLogoesRepository aboutUsLogoesRepository = new AboutUsLogoesRepository();
        public IActionResult Index()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "تنظیمات"))
            {
                var content = aboutUsLogoesRepository.GetAboutUsLogos();
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
        public IActionResult Create(AboutUsLogo logo, IFormFile imageProduct)
        {
            if (aboutUsLogoesRepository.Create(logo, imageProduct))
            {
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                return View();
            }
        }

        [Route("Update/{logoID}")]
        public IActionResult Update(int logoID)
        {
            var content = aboutUsLogoesRepository.GetByID(logoID);
            return View(content);
        }

        [HttpPost]
        [Route("Update/{logoID}")]
        public IActionResult Update(AboutUsLogo logo, IFormFile imageProduct)
        {
            if (aboutUsLogoesRepository.Update(logo, imageProduct))
            {
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = aboutUsLogoesRepository.GetByID(logo.AboutUsLogoId);
                return View(content);
            }
        }

        [Route("Delete/{logoID}")]
        public IActionResult Delete(int logoID)
        {
            var content = aboutUsLogoesRepository.GetByID(logoID);
            return View(content);
        }

        [HttpPost]
        [Route("Delete/{logoID}")]
        public IActionResult Delete(AboutUsLogo logo)
        {
            if (aboutUsLogoesRepository.Delete(logo))
            {
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = aboutUsLogoesRepository.GetByID(logo.AboutUsLogoId);
                return View(content);
            }
        }
    }
}
