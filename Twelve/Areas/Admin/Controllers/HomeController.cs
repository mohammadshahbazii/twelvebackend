using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services;

namespace Twelve.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin")]
    public class HomeController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();
        ISiteRepository siteRepository = new SiteRepository();

        public IActionResult Index()
        {
            var count=adminRepository.GetSiteVisitData().Values;
            ViewBag.Counts = JsonConvert.SerializeObject(count);

            var content = adminRepository.GetStatistics();
            return View(content);
        }

        [Route("Settings")]
        public IActionResult Settings()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "تنظیمات"))
            {
                var content = siteRepository.GetSiteSetting();
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [Route("Settings")]
        [HttpPost]
        [HttpPost, DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = Int32.MaxValue, ValueLengthLimit = Int32.MaxValue)]
        public IActionResult Settings(SiteSetting setting, IFormFile file , IFormFile imageProduct)
        {
            if (siteRepository.UpdateSite(setting, file , imageProduct))
            {
                ViewBag.Message = "عملیات با موفقیت انجام شد";
                var content = siteRepository.GetSiteSetting();
                return View(content);
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = siteRepository.GetSiteSetting();
                return View(content);
            }
        }

        [Route("IndexContent")]
        public IActionResult IndexContent()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "تنظیمات"))
            {
                var content = siteRepository.GetIndexContent();
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("IndexContent")]
        public IActionResult IndexContent(IndexContent indexContent)
        {
            if (siteRepository.UpdateIndexContent(indexContent))
            {
                ViewBag.Message = "عملیات با موفقیت انجام شد";
                var content = siteRepository.GetIndexContent();
                return View(content);
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = siteRepository.GetIndexContent();
                return View(content);
            }
        }

        [Route("AboutUsContent")]
        public IActionResult AboutUsContent()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "تنظیمات"))
            {
                var content = siteRepository.GetAboutUsContent();
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("AboutUsContent")]
        public IActionResult AboutUsContent(AboutUsContent aboutUsContent, IFormFile imageProduct , IFormFile downloadProduct)
        {
            if (siteRepository.UpdateAboutUsContent(aboutUsContent,imageProduct,downloadProduct))
            {
                ViewBag.Message = "عملیات با موفقیت انجام شد";
                var content = siteRepository.GetAboutUsContent();
                return View(content);
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = siteRepository.GetAboutUsContent();
                return View(content);
            }
        }

        [Route("ContactUsContent")]
        public IActionResult ContactUsContent()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "تنظیمات"))
            {
                var content = siteRepository.GetContactUsContent();
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("ContactUsContent")]
        public IActionResult ContactUsContent(ContactUsContent contactUsContent)
        {
            if (siteRepository.UpdateContactUsContent(contactUsContent))
            {
                ViewBag.Message = "عملیات با موفقیت انجام شد";
                var content = siteRepository.GetContactUsContent();
                return View(content);
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = siteRepository.GetContactUsContent();
                return View(content);
            }
        }

        [Route("FaqContent")]
        public IActionResult FaqContent()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "تنظیمات"))
            {
                IFaqsRepository faqsRepository = new FaqsRepository();
                var content = faqsRepository.GetFaqContent();
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("FaqContent")]
        public IActionResult FaqContent(FaqContent aboutUsContent)
        {
            IFaqsRepository faqsRepository = new FaqsRepository();

            if (faqsRepository.UpdateFaqContent(aboutUsContent))
            {
                ViewBag.Message = "عملیات با موفقیت انجام شد";
                var content = faqsRepository.GetFaqContent();
                return View(content);
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = faqsRepository.GetFaqContent();
                return View(content);
            }
        }

        [Route("AppMenuContent")]
        public IActionResult AppMenuContent()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "تنظیمات"))
            {
                IFeaturesRepository featuresRepository = new FeaturesRepository();
                var content = featuresRepository.GetFeaturesContent();
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("AppMenuContent")]
        public IActionResult AppMenuContent(FeaturesContent featuresContent)
        {
            IFeaturesRepository featuresRepository = new FeaturesRepository();

            if (featuresRepository.UpdateFeaturesContent(featuresContent))
            {
                ViewBag.Message = "عملیات با موفقیت انجام شد";
                var content = featuresRepository.GetFeaturesContent();
                return View(content);
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = featuresRepository.GetFeaturesContent();
                return View(content);
            }
        }

    }
}
