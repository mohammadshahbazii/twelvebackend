using System.IO;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Twelve.Models;
using Services;
using Grpc.Core;
using Utilities;
using Microsoft.Extensions.Localization;

namespace Twelve.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            ISiteRepository siteRepository = new SiteRepository();
            siteRepository.PlusVisit();
            ViewData["WelcomeMessage"] = _localizer["WelcomeMessage"];
            return View();
        }

        [Route("SendSms")]
        public string SendSms(string Phonenumber)
        {
            var result = Utilities.SendSms.Send(Phonenumber);
            return result.Result;

        }

        [Route("ContactUs")]
        public IActionResult ContactUs()
        {
            if (HttpContext.Session.Keys.Any(k => k == "PaymentMessage"))
            {
                ViewBag.Message = HttpContext.Session.GetString("PaymentMessage");

            }
            ISiteRepository siteRepository = new SiteRepository();
            var content = siteRepository.GetSocialMedia();
            return View(content);
        }

        [Route("AboutUs")]
        public IActionResult AboutUs()
        {
            ISiteRepository siteRepository = new SiteRepository();
            var content = siteRepository.GetAboutUsContent();
            return View(content);
        }

        [Route("Faq")]
        public IActionResult Faq()
        {
            return View();
        }

        [Route("GetIntroduceSlider/{featureID}")]
        public IActionResult GetIntroduceSlider(int featureID)
        {
            return ViewComponent("IntroduceAppSlider",new { FeatureID = featureID });
        }

        [HttpGet]
        [Route(".well-known/assetlinks.json")]
        public IActionResult GetAssetLinks()
        {
            ISiteRepository siteRepository = new SiteRepository();
            var settings = siteRepository.GetSiteSetting();

            if (settings == null || string.IsNullOrWhiteSpace(settings.JsonFileName))
            {
                return Content("[]", "application/json");
            }

            return Content(settings.JsonFileName, "application/json");
        }

        [Route("SubmitMessage")]
        public string SubmitMessage(string fullname, string Email, string Phonenumber, string Subject, string Text)
        {
            ISiteRepository siteRepository = new SiteRepository();
            return siteRepository.SubmitMessage(fullname, Email, Phonenumber, Subject, Text);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("Download")]
        public IActionResult Download()
        {
            using (var siteRepository = new SiteRepository())
            {
                siteRepository.AddDownloadCount();
                return File("/Files/" + siteRepository.GetDirectLink(), System.Net.Mime.MediaTypeNames.Application.Octet, "Twelve.apk");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}