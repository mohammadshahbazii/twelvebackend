using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Text.RegularExpressions;

namespace Twelve.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/Sliders")]
    public class SlidersController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();
        ISlidersRepository slidersRepository = new SlidersRepository();
        public IActionResult Index()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "اسلایدر ها"))
            {
                var content = slidersRepository.GetSliderGroups();
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }
        [Route("ShowItems/{GroupID}")]
        public IActionResult ShowItems(int GroupID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "اسلایدر ها"))
            {
                var content = slidersRepository.GetSliders(GroupID);
                ViewBag.GroupID = GroupID;
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [Route("Create/{GroupID}")]
        public IActionResult Create(int GroupID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "اسلایدر ها"))
            {
                ViewBag.GroupID = GroupID;
                return View();
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Create/{GroupID}")]
        public IActionResult Create(Slider slider, IFormFile imageProduct)
        {
            if (slidersRepository.Create(slider, imageProduct))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.GroupID = slider.SliderGroupId;
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                return View();
            }
        }

        [Route("Update/{InfoID}")]
        public IActionResult Update(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "اسلایدر ها"))
            {
                var content = slidersRepository.GetByID(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Update/{InfoID}")]
        public IActionResult Update(Slider slider, IFormFile imageProduct)
        {
            if (slidersRepository.Update(slider, imageProduct))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = slidersRepository.GetByID(slider.SliderId);
                return View(content);
            }
        }

        [Route("Delete/{InfoID}")]
        public IActionResult Delete(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "اسلایدر ها"))
            {
                var content = slidersRepository.GetByID(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Delete/{InfoID}")]
        public IActionResult Delete(Slider slider)
        {
            if (slidersRepository.Delete(slider))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = slidersRepository.GetByID(slider.SliderId);
                return View(content);
            }
        }
    }
}
