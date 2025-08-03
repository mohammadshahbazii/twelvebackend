using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using ViewModels;

namespace Twelve.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/BlogGroups")]
    public class BlogGroupsController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();
        IBlogGroupsRepository blogGroupsRepository = new BlogGroupsRepository();

        public IActionResult Index()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "دسته بندی های اخبار"))
            {
                var content = blogGroupsRepository.GetAllGroups();
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
                return View(new BlogGroupCrudViewModel());
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(BlogGroupCrudViewModel blogGroup)
        {
            var entity = new BlogGroup { GroupName = blogGroup.GroupName, ParentId = blogGroup.ParentId };
            if (blogGroupsRepository.Create(entity))
            {
                blogGroup.BlogGroupId = entity.BlogGroupId;
                blogGroupsRepository.SaveTranslations(blogGroup);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                return View(blogGroup);
            }
        }

        [Route("Update/{InfoID}")]
        public IActionResult Update(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "دسته بندی های اخبار"))
            {
                var content = blogGroupsRepository.GetForEdit(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Update/{InfoID}")]
        public IActionResult Update(BlogGroupCrudViewModel blogGroup)
        {
            var entity = new BlogGroup { BlogGroupId = blogGroup.BlogGroupId, GroupName = blogGroup.GroupName, ParentId = blogGroup.ParentId };
            if (blogGroupsRepository.Update(entity))
            {
                blogGroupsRepository.SaveTranslations(blogGroup);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = blogGroupsRepository.GetForEdit(blogGroup.BlogGroupId);
                return View(content);
            }
        }

        [Route("Delete/{InfoID}")]
        public IActionResult Delete(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "دسته بندی های اخبار"))
            {
                var content = blogGroupsRepository.GetByID(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Delete/{InfoID}")]
        public IActionResult Delete(BlogGroup blogGroup)
        {
            if (blogGroupsRepository.Delete(blogGroup))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = blogGroupsRepository.GetByID(blogGroup.BlogGroupId);
                return View(content);
            }
        }
    }
}
