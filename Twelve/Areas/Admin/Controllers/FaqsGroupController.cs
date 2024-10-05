using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/FaqsGroup")]
    public class FaqsGroupController : Controller
    {
        
        IAdminRepository adminRepository = new AdminRepository();
        IFaqsRepository faqsRepository = new FaqsRepository();
        public IActionResult Index()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "دسته بندی های اخبار"))
            {
                var content = faqsRepository.GetFaqGroups();
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
            if (adminRepository.CheckPermission(User.Identity.Name, "دسته بندی های اخبار"))
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
        public IActionResult Create(FaqGroup faqGroup)
        {
            if (faqsRepository.CreateGroup(faqGroup))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                return View();
            }
        }

        [Route("Update/{InfoID}")]
        public IActionResult Update(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "دسته بندی های اخبار"))
            {
                var content = faqsRepository.GetGroupByID(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Update/{InfoID}")]
        public IActionResult Update(FaqGroup faqGroup)
        {
            if (faqsRepository.UpdateGroup(faqGroup))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = faqsRepository.GetByID(faqGroup.FaqGroupId);
                return View(content);
            }
        }

        [Route("Delete/{InfoID}")]
        public IActionResult Delete(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "دسته بندی های اخبار"))
            {
                var content = faqsRepository.GetGroupByID(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Delete/{InfoID}")]
        public IActionResult Delete(FaqGroup faqGroup)
        {
            if (faqsRepository.DeleteGroup(faqGroup))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = faqsRepository.GetByID(faqGroup.FaqGroupId);
                return View(content);
            }
        }
    }
}
