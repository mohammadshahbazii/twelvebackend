using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using ViewModels;

namespace Twelve.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/Blog")]
    public class BlogController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();
        IBlogRepository blogRepository = new BlogRepository();

        public IActionResult Index()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "لیست اخبار"))
            {
                var content = blogRepository.GetBlogs();
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [Route("RelatedPosts/{BlogID}")]
        public IActionResult RelatedPosts(int BlogID)
        {
            ViewBag.BlogID = BlogID;
            var content = blogRepository.GetRelatedPosts(BlogID);
            return View(content);
        }

        [Route("CreateRelated/{BlogID}")]
        public IActionResult CreateRelated(int BlogID)
        {
            var content = blogRepository.GetModelForCreateRelatedBlog(BlogID);
            return View(content);
        }

        [HttpPost]
        [Route("CreateRelated/{BlogID}")]
        public IActionResult CreateRelated(CreateRelatedBlogsViewModel blogs)
        {
            if (blogRepository.CreateRelatedBlogs(blogs))
            {
                return RedirectToAction("RelatedPosts", new { BLogID = blogs.BlogID });
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = blogRepository.GetModelForCreateRelatedBlog(blogs.BlogID);
                return View(content);
            }
        }

        [Route("GetBlogRelatedPosts")]
        public IActionResult GetBlogRelatedPosts(int BlogID,string q = "")
        {
            return ViewComponent("AdminBlogRelatedPosts", new {  BlogID = BlogID, q = q });
        }

        [Route("GetBlogPosts")]
        public IActionResult GetBlogPosts(string q = "", int PageID = 1)
        {
            return ViewComponent("AdminBlogPostsItem", new { q = q, PageID = PageID });
        }

        [Route("Create")]
        public IActionResult Create()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "لیست اخبار"))
            {
                var content = blogRepository.GetModelForCreate();
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
            if (blog.SelectedGroups == null)
            {
                ViewBag.SelectedMessage = "لطفا دسته بندی مدنظر خود را وارد کنید";
                var content = blogRepository.GetModelForCreate();
                return View(content);
            }
            if (imageProduct == null)
            {
                ViewBag.Message = "لطفا تصویر مدنظر خود را وارد کنید";
                var content = blogRepository.GetModelForCreate();
                return View(content);
            }
            if (blogRepository.Create(blog,imageProduct))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = blogRepository.GetModelForCreate();
                return View(content);
            }
        }

        [Route("Update/{InfoID}")]
        public IActionResult Update(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "لیست اخبار"))
            {
                var content = blogRepository.GetByID(InfoID);
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
            if (blog.SelectedGroups == null)
            {
                ViewBag.SelectedMessage = "لطفا دسته بندی مدنظر خود را وارد کنید";
                var content = blogRepository.GetModelForCreate();
                return View(content);
            }
            if (blogRepository.Update(blog , imageProduct))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = blogRepository.GetByID(blog.BlogID);
                return View(content);
            }
        }

        [Route("Delete/{InfoID}")]
        public IActionResult Delete(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "لیست اخبار"))
            {
                var content = blogRepository.GetByID(InfoID);
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
            if (blogRepository.Delete(blog.BlogID))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = blogRepository.GetByID(blog.BlogID);
                return View(content);
            }
        }
    }
}
