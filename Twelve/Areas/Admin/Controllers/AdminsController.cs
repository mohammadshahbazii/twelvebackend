using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using ViewModels;

namespace Twelve.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/Admins")]
    public class AdminsController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();

        
        public IActionResult Index()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "لیست ادمین ها"))
            {
                var content = adminRepository.GetAdmins();
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [Route("GetAdmins")]
        public IActionResult GetAdmins(string q = "", int PageID = 1)
        {
            return ViewComponent("AdminsItem", new { q = q, PageID = PageID });
        }

        [Route("Create")]
        public IActionResult Create()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "لیست ادمین ها"))
            {
                var content = adminRepository.GetModelForCreate();
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(AdminCrudViewModel admin)
        {
            if (adminRepository.Create(admin))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = adminRepository.GetModelForCreate();
                return View(content);
            }
        }

        [Route("Update/{InfoID}")]
        public IActionResult Update(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "لیست ادمین ها"))
            {
                var content = adminRepository.GetByID(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Update/{InfoID}")]
        public IActionResult Update(AdminCrudViewModel admin)
        {
            if (adminRepository.Update(admin))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = adminRepository.GetByID(admin.AdminID);
                return View(content);
            }
        }

        [Route("Delete/{InfoID}")]
        public IActionResult Delete(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "لیست ادمین ها"))
            {
                var content = adminRepository.GetByID(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Delete/{InfoID}")]
        public IActionResult Delete(AdminCrudViewModel Admin)
        {
            if (adminRepository.Delete(Admin))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = adminRepository.GetByID(Admin.AdminID);
                return View(content);
            }
        }
    }
}
