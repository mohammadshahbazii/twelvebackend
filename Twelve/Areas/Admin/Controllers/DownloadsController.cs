using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using ViewModels;

namespace Twelve.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/Downloads")]
    public class DownloadsController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();
        ISiteRepository siteRepository = new SiteRepository();
        public IActionResult Index()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "لینک های دانلود"))
            {
                var content = siteRepository.GetDownloadLinks();
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
            if (adminRepository.CheckPermission(User.Identity.Name, "لینک های دانلود"))
            {
                var content = siteRepository.GetDownloadLinkModelForCreate();
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(DownloadLinkCrudViewModel downloadLink, IFormFile imageProduct)
        {
            if (downloadLink.SelectedGroups == null)
            {
                ViewBag.Message = "لطفا دسته بندی مدنظر خود را انتخاب کنید";
                var content = siteRepository.GetDownloadLinkModelForCreate();
                return View(content);
            }
            if (imageProduct == null)
            {
                ViewBag.Message = "لطفا تصویر مدنظر خود را انتخاب کنید";
                var content = siteRepository.GetDownloadLinkModelForCreate();
                return View(content);
            }
            if (siteRepository.Create(downloadLink, imageProduct))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = siteRepository.GetDownloadLinkModelForCreate();
                return View(content);
            }
        }

        [Route("Update/{InfoID}")]
        public IActionResult Update(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "لینک های دانلود"))
            {
                var content = siteRepository.GetDownloadLinkModelForUpdate(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Update/{InfoID}")]
        public IActionResult Update(DownloadLinkCrudViewModel downloadlink, IFormFile imageProduct)
        {
            if (!downloadlink.SelectedGroups.Any())
            {
                ViewBag.Message = "لطفا دسته بندی مدنظر خود را انتخاب کنید";
                var content = siteRepository.GetDownloadLinkModelForUpdate(downloadlink.DownloadID);
                return View(content);
            }
            if (ModelState.IsValid)
            {
                if (siteRepository.Update(downloadlink, imageProduct))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                    var content = siteRepository.GetDownloadLinkModelForUpdate(downloadlink.DownloadID);
                    return View(content);
                }
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = siteRepository.GetDownloadLinkModelForUpdate(downloadlink.DownloadID);
                return View(content);
            }


        }

        [Route("Delete/{InfoID}")]
        public IActionResult Delete(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "لینک های دانلود"))
            {
                var content = siteRepository.GetDownloadLink(InfoID);
                ViewBag.GroupName = siteRepository.GetDownloadLinkGroup(content.DlgroupId).GroupName;
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Delete/{InfoID}")]
        public IActionResult Delete(DownloadLink downloadlink)
        {
            if (siteRepository.Delete(downloadlink))
            {
                return RedirectToAction("Index");
            }
            else
            {
                var content = siteRepository.GetDownloadLink(downloadlink.DownloadLinkId);
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                return View(content);
            }
        }
    }
}
