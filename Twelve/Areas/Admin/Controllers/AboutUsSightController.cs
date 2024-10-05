using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using ViewModels;

namespace Twelve.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/AboutUsSight")]
    public class AboutUsSightController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();
        IAboutUsSightRepository aboutUsSightRepository = new AboutUsSightRepository();

        public IActionResult Index()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "تنظیمات"))
            {
                var content = aboutUsSightRepository.GetAboutUsSights();
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
        public IActionResult Create(AboutUsSight sight, IFormFile imageProduct)
        {
            if (aboutUsSightRepository.Create(sight, imageProduct))
            {
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                return View();
            }
        }

        [Route("Update/{SightID}")]
        public IActionResult Update(int sightID)
        {
            var content = aboutUsSightRepository.GetByID(sightID);
            return View(content);
        }

        [HttpPost]
        [Route("Update/{SightID}")]
        public IActionResult Update(AboutUsSight sight , IFormFile imageProduct)
        {
            if (aboutUsSightRepository.Update(sight, imageProduct))
            {
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = aboutUsSightRepository.GetByID(sight.AboutUsSightId);
                return View(content);
            }
        }

        [Route("Delete/{SightID}")]
        public IActionResult Delete(int sightID)
        {
            var content = aboutUsSightRepository.GetByID(sightID);
            return View(content);
        }

        [HttpPost]
        [Route("Delete/{SightID}")]
        public IActionResult Delete(AboutUsSight sight)
        {
            if (aboutUsSightRepository.Delete(sight))
            {
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = aboutUsSightRepository.GetByID(sight.AboutUsSightId);
                return View(content);
            }
        }
    }
}
