using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using ViewModels;

namespace Twelve.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/News")]
    public class NewsController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();
        IBlogRepository blogRepository = new BlogRepository();
        public IActionResult Index()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "لیست اخبار اپلیکیشن"))
            {
                var content = blogRepository.GetNews();
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [Route("GetNewsPosts")]
        public IActionResult GetNewsPosts(string q = "", int PageID = 1)
        {
            return ViewComponent("AdminGetNewsPostsItem", new { q = q, PageID = PageID });
        }

        [Route("Create")]
        public IActionResult Create()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "لیست اخبار اپلیکیشن"))
            {
                var content = blogRepository.GetNewsModelForCreate();
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(BlogCrudViewModel blog, IFormFile imageProduct)
        {
            if (blogRepository.CreateNews(blog, imageProduct))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = blogRepository.GetNewsModelForCreate();
                return View(content);
            }
        }

        [Route("Update/{InfoID}")]
        public IActionResult Update(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "لیست اخبار اپلیکیشن"))
            {
                var content = blogRepository.GetNewsByID(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Update/{InfoID}")]
        public IActionResult Update(BlogCrudViewModel blog, IFormFile imageProduct)
        {
            if (blogRepository.UpdateNews(blog, imageProduct))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = blogRepository.GetNewsByID(blog.BlogID);
                return View(content);
            }
        }

        [Route("Delete/{InfoID}")]
        public IActionResult Delete(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "لیست اخبار اپلیکیشن"))
            {
                var content = blogRepository.GetNewsByID(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Delete/{InfoID}")]
        public IActionResult Delete(BlogCrudViewModel blog)
        {
            if (blogRepository.DeleteNews(blog.BlogID))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = blogRepository.GetNewsByID(blog.BlogID);
                return View(content);
            }
        }
    }
}
