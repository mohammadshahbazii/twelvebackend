using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using ViewModels;

namespace Twelve.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/Introduces")]
    public class IntroducesController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();
        IIntroducesRepository introducesRepository = new IntroducesRepository();

        public IActionResult Index()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "ویژگی ها"))
            {
                var content = introducesRepository.GetIntroduces();
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
            if (adminRepository.CheckPermission(User.Identity.Name, "ویژگی ها"))
            {
                var content = introducesRepository.GetModelForCreate();
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(IntroduceCrudViewModel introduce)
        {
            if (introducesRepository.Create(introduce))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = introducesRepository.GetModelForCreate();
                return View(content);
            }
        }

        [Route("Update/{IntroduceID}")]
        public IActionResult Update(int introduceID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "ویژگی ها"))
            {
                var content = introducesRepository.GetByID(introduceID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Update/{IntroduceID}")]
        public IActionResult Update(IntroduceCrudViewModel introduce)
        {
            if (introducesRepository.Update(introduce))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = introducesRepository.GetByID(introduce.IntroduceID);
                return View(content);
            }
        }

        [Route("Delete/{IntroduceID}")]
        public IActionResult Delete(int introduceID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "ویژگی ها"))
            {
                var content = introducesRepository.GetByID(introduceID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Delete/{IntroduceID}")]
        public IActionResult Delete(IntroduceCrudViewModel introduce)
        {
            if (introducesRepository.Delete(introduce.IntroduceID))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = introducesRepository.GetByID(introduce.IntroduceID);
                return View(content);
            }
        }
    }
}
