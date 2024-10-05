using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/BlogComments")]
    public class BlogCommentsController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();
        IBlogRepository blogRepository = new BlogRepository();

        public IActionResult Index()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "دیدگاه های اخبار"))
            {
                var content = blogRepository.GetComments();
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [Route("GetComments")]
        public IActionResult GetComments(string q = "", int PageID = 1)
        {
            return ViewComponent("AdminblogCommentsItem", new { q = q, PageID = PageID });
        }

        [Route("ShowAnswer/{CommentID}")]
        public IActionResult ShowAnswer(int commentID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "دیدگاه های اخبار"))
            {
                var content = blogRepository.GetAnswerByID(commentID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [Route("ConfirmComments")]
        public IActionResult ConfirmComments()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "دیدگاه های اخبار"))
            {
                var content = blogRepository.GetConfirmComments();
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [Route("GetConfirmComments")]
        public IActionResult GetConfirmComments(string q = "", int PageID = 1)
        {
            return ViewComponent("AdminblogConfirmCommentsItem", new { q = q, PageID = PageID });
        }

        [Route("AnswerComments")]
        public IActionResult AnswerComments()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "دیدگاه های اخبار"))
            {
                var content = blogRepository.GetAnswerComments();
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [Route("GetAnswerComments")]
        public IActionResult GetAnswerComments(string q = "", int PageID = 1)
        {
            return ViewComponent("AdminblogAnswerCommentsItem", new { q = q, PageID = PageID });
        }

        [Route("Answer/{CommentID}")]
        public IActionResult Answer(int commentID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "دیدگاه های اخبار"))
            {
                ViewBag.CommentsID = commentID;
                ViewBag.BlogID = blogRepository.GetCommentByID(commentID).BlogId;
                return View();
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Answer/{CommentID}")]
        public IActionResult Answer(BlogComment comment)
        {
            if (blogRepository.AnswerComment(comment))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                return View();
            }
        }

        [Route("Confirm/{InfoID}")]
        public IActionResult Confirm(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "دیدگاه های اخبار"))
            {
                var content = blogRepository.GetCommentByID(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Confirm/{InfoID}")]
        public IActionResult Confirm(BlogComment comment)
        {
            if (blogRepository.UpdateComment(comment))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = blogRepository.GetCommentByID(comment.CommentId);
                return View(content);
            }
        }

        [Route("Delete/{InfoID}")]
        public IActionResult Delete(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "دیدگاه های اخبار"))
            {
                var content = blogRepository.GetCommentByID(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Delete/{InfoID}")]
        public IActionResult Delete(BlogComment comment)
        {
            if (blogRepository.Delete(comment))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = blogRepository.GetCommentByID(comment.CommentId);
                return View(content);
            }
        }
    }
}
