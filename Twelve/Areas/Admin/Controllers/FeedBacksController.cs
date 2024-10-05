using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/FeedBacks")]
    public class FeedBacksController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();
        IFeedBacksRepository feedBacksRepository = new FeedBacksRepository();

        public IActionResult Index()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "پیشنهادات کاربران"))
            {
                var content = feedBacksRepository.GetFeedBacks();
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [Route("GetFeedBacks")]
        public IActionResult GetFeedBacks(string q = "", int PageID = 1)
        {
            return ViewComponent("AdminGetFeedBacksItem", new { q = q, PageID = PageID });
        }

        [Route("ShowMessage/{MessageID}")]
        public IActionResult ShowMessage(int messageID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "پیشنهادات کاربران"))
            {
                var content = feedBacksRepository.GetByID(messageID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [Route("Selected")]
        public IActionResult Selected()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "پیشنهادات کاربران"))
            {
                var content = feedBacksRepository.GetAdminSelectedFeedBacks();
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [Route("SetShow/{InfoID}")]
        public IActionResult SetShow(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "پیشنهادات کاربران"))
            {
                var content = feedBacksRepository.GetByID(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("SetShow/{InfoID}")]
        public IActionResult SetShow(FeedBack feedBack, IFormFile imageProduct)
        {
            if (feedBacksRepository.Update(feedBack, imageProduct))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = feedBacksRepository.GetByID(feedBack.FeedBackId);
                return View(content);
            }
        }

        [Route("Delete/{InfoID}")]
        public IActionResult Delete(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "پیشنهادات کاربران"))
            {
                var content = feedBacksRepository.GetByID(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Delete/{InfoID}")]
        public IActionResult Delete(FeedBack feedBack)
        {
            if (feedBacksRepository.Delete(feedBack))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = feedBacksRepository.GetByID(feedBack.FeedBackId);
                return View(content);
            }
        }
    }
}
