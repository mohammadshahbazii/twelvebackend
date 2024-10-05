using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/SocialMedia")]
    public class SocialMediaController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();
        ISiteRepository siteRepository = new SiteRepository();
        public IActionResult Index()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "صفحات مجازی"))
            {
                var content = siteRepository.GetSocialMedia();
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
            if (adminRepository.CheckPermission(User.Identity.Name, "اطلاعات تماس"))
            {
                return View();
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(SocialMedium socail)
        {
            if (siteRepository.CreateMedia(socail))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                return View();
            }
        }

        [Route("Update/{MediaID}")]
        public IActionResult Update(int MediaID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "صفحات مجازی"))
            {
                var content = siteRepository.GetMedia(MediaID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Update/{MediaID}")]
        public IActionResult Update(SocialMedium social)
        {
            if (siteRepository.UpdateMedia(social))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = siteRepository.GetMedia(social.SocialMediaId);
                return View(content);
            }
        }

        [Route("Delete/{MediaID}")]
        public IActionResult Delete(int MediaID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "صفحات مجازی"))
            {
                var content = siteRepository.GetMedia(MediaID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Delete/{MediaID}")]
        public IActionResult Delete(SocialMedium social)
        {
            if (siteRepository.DeleteMedia(social))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = siteRepository.GetMedia(social.SocialMediaId);
                return View(content);
            }
        }
    }
}
