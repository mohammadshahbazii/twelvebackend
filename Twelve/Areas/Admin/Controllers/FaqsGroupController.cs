using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using ViewModels;

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
                return View(new FaqGroupCrudViewModel());
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(FaqGroupCrudViewModel faqGroup)
        {
            FaqGroup entity = new FaqGroup { GroupName = faqGroup.GroupName, ParentId = faqGroup.ParentId };
            if (faqsRepository.CreateGroup(entity))
            {
                faqGroup.FaqGroupId = entity.FaqGroupId;
                faqsRepository.SaveGroupTranslations(faqGroup);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                return View(faqGroup);
            }
        }

        [Route("Update/{InfoID}")]
        public IActionResult Update(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "دسته بندی های اخبار"))
            {
                var content = faqsRepository.GetGroupForEdit(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Update/{InfoID}")]
        public IActionResult Update(FaqGroupCrudViewModel faqGroup)
        {
            FaqGroup entity = new FaqGroup { FaqGroupId = faqGroup.FaqGroupId, GroupName = faqGroup.GroupName, ParentId = faqGroup.ParentId };
            if (faqsRepository.UpdateGroup(entity))
            {
                faqsRepository.SaveGroupTranslations(faqGroup);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = faqsRepository.GetGroupForEdit(faqGroup.FaqGroupId);
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
