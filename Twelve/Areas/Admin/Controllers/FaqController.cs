using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using ViewModels;

namespace Twelve.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/Faq")]
    public class FaqController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();
        IFaqsRepository faqsRepository = new FaqsRepository();
        public IActionResult Index()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "لیست سوالات متداول"))
            {
                var content = faqsRepository.GetFaqs();
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [Route("GetFaqs")]
        public IActionResult GetFaqs(string q = "", int PageID = 1)
        {
            return ViewComponent("AdminFaqsItem", new { q = q, PageID = PageID });
        }

        [Route("Create")]
        public IActionResult Create()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "لیست سوالات متداول"))
            {
                var content = faqsRepository.GetModelForCreate();
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(FaqsCrudViewModel faqs)
        {
            if (faqsRepository.Create(faqs))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = faqsRepository.GetModelForCreate();
                return View(content);
            }
        }

        [Route("Update/{InfoID}")]
        public IActionResult Update(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "لیست سوالات متداول"))
            {
                var content = faqsRepository.GetByID(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Update/{InfoID}")]
        public IActionResult Update(FaqsCrudViewModel faqs)
        {
            if (faqsRepository.Update(faqs))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = faqsRepository.GetByID(faqs.FaqID);
                return View(content);
            }
        }

        [Route("Delete/{InfoID}")]
        public IActionResult Delete(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "لیست سوالات متداول"))
            {
                var content = faqsRepository.GetByID(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Delete/{InfoID}")]
        public IActionResult Delete(FaqsCrudViewModel faqs)
        {
            if (faqsRepository.Delete(faqs.FaqID))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = faqsRepository.GetByID(faqs.FaqID);
                return View(content);
            }
        }
    }
}
