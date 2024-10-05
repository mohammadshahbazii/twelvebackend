using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/IntroduceSliders")]
    public class IntroduceSlidersController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();
        IFeaturesRepository featuresRepository = new FeaturesRepository();
        IIntroduceSlidersRepository introduceSlidersRepository = new IntroduceSlidersRepository();

        public IActionResult Index()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "تنظیمات"))
            {
                var content = featuresRepository.GetAdminFeatures();
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [Route("GetFeatures")]
        public IActionResult GetFeatures(string q = "", int PageID = 1)
        {
            return ViewComponent("AdminIntroduceFeaturesItem", new { q = q, PageID = PageID });
        }

        [Route("ShowItems/{featureID}")]
        public IActionResult ShowItems(int featureID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "تنظیمات"))
            {
                var content = introduceSlidersRepository.GetListByFeatureID(featureID);
                ViewBag.FeatureID = featureID;
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [Route("Create/{FeatureID}")]
        public IActionResult Create(int featureID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "تنظیمات"))
            {
                return View(new IntroduceSlider() { FeatureId = featureID });
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Create/{FeatureID}")]
        public IActionResult Create(IntroduceSlider introduceSlider, IFormFile imageProduct)
        {
            if (introduceSlidersRepository.Create(introduceSlider, imageProduct))
            {
                return RedirectToAction("ShowItems",new {FeatureID = introduceSlider.FeatureId });
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                return View(new IntroduceSlider() { FeatureId = introduceSlider.FeatureId });
            }
        }

        [Route("Update/{sliderID}")]
        public IActionResult Update(int sliderID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "اطلاعات تماس"))
            {
                var content = introduceSlidersRepository.GetByID(sliderID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Update/{sliderID}")]
        public IActionResult Update(IntroduceSlider slider, IFormFile imageProduct)
        {
            if (introduceSlidersRepository.Update(slider, imageProduct))
            {
                return RedirectToAction("ShowItems",new {FeatureID = slider.FeatureId });
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = introduceSlidersRepository.GetByID(slider.IntroduceSliderId);
                return View(content);
            }
        }

        [Route("Delete/{sliderID}")]
        public IActionResult Delete(int sliderID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "اطلاعات تماس"))
            {
                var content = introduceSlidersRepository.GetByID(sliderID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Delete/{sliderID}")]
        public IActionResult Delete(IntroduceSlider slider)
        {
            if (introduceSlidersRepository.Delete(slider))
            {
                return RedirectToAction("ShowItems", new { FeatureID = slider.FeatureId });
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = introduceSlidersRepository.GetByID(slider.IntroduceSliderId);
                return View(content);
            }
        }
    }
}
