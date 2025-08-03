using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Text.RegularExpressions;
using ViewModels;

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
                return View(new SliderCrudViewModel { SliderGroupId = GroupID });
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Create/{GroupID}")]
        public IActionResult Create(SliderCrudViewModel slider, IFormFile imageProduct)
        {
            var entity = new Slider
            {
                SliderGroupId = slider.SliderGroupId,
                Title = slider.Title,
                ShortDescription = slider.ShortDescription,
                Link = slider.Link
            };
            if (slidersRepository.Create(entity, imageProduct))
            {
                slider.SliderId = entity.SliderId;
                slidersRepository.SaveTranslations(slider);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                return View(slider);
            }
        }

        [Route("Update/{InfoID}")]
        public IActionResult Update(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "اسلایدر ها"))
            {
                var content = slidersRepository.GetForEdit(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Update/{InfoID}")]
        public IActionResult Update(SliderCrudViewModel slider, IFormFile imageProduct)
        {
            var entity = new Slider
            {
                SliderId = slider.SliderId,
                SliderGroupId = slider.SliderGroupId,
                Title = slider.Title,
                ShortDescription = slider.ShortDescription,
                Link = slider.Link,
                ImageName = slider.ImageName
            };
            if (slidersRepository.Update(entity, imageProduct))
            {
                slidersRepository.SaveTranslations(slider);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = slidersRepository.GetForEdit(slider.SliderId);
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
