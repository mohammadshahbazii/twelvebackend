using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/AboutUsArticles")]
    public class AboutUsArticlesController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();
        IAboutUsArticlesRepository aboutUsArticlesRepository = new AboutUsArticlesRepository();
        public IActionResult Index()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "تنظیمات"))
            {
                var content = aboutUsArticlesRepository.GetAboutUsArticles();
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
            return View();
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(AboutUsArticle article, IFormFile imageProduct)
        {
            if (aboutUsArticlesRepository.Create(article, imageProduct))
            {
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                return View();
            }
        }

        [Route("Update/{ArticleID}")]
        public IActionResult Update(int articleID)
        {
            var content = aboutUsArticlesRepository.GetByID(articleID);
            return View(content);
        }

        [HttpPost]
        [Route("Update/{ArticleID}")]
        public IActionResult Update(AboutUsArticle article, IFormFile imageProduct)
        {
            if (aboutUsArticlesRepository.Update(article, imageProduct))
            {
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = aboutUsArticlesRepository.GetByID(article.AboutUsArticleId);
                return View(content);
            }
        }

        [Route("Delete/{ArticleID}")]
        public IActionResult Delete(int articleID)
        {
            var content = aboutUsArticlesRepository.GetByID(articleID);
            return View(content);
        }

        [HttpPost]
        [Route("Delete/{ArticleID}")]
        public IActionResult Delete(AboutUsArticle article)
        {
            if (aboutUsArticlesRepository.Delete(article))
            {
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = aboutUsArticlesRepository.GetByID(article.AboutUsArticleId);
                return View(content);
            }
        }
    }
}
