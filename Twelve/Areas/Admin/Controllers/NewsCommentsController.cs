using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/NewsComments")]
    public class NewsCommentsController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();
        IBlogRepository blogRepository = new BlogRepository();
        public IActionResult Index()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "دیدگاه های اخبار اپلیکیشن"))
            {
                var content = blogRepository.GetNewsComments();
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
            return ViewComponent("AdminNewsCommentsItem", new { q = q, PageID = PageID });
        }

        [Route("ConfirmComments")]
        public IActionResult ConfirmComments()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "دیدگاه های اخبار اپلیکیشن"))
            {
                var content = blogRepository.GetNewsConfirmComments();
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
            return ViewComponent("AdminNewsConfirmCommentsItem", new { q = q, PageID = PageID });
        }

        [Route("AnswerComments")]
        public IActionResult AnswerComments()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "دیدگاه های اخبار اپلیکیشن"))
            {
                var content = blogRepository.GetNewsAnswerComments();
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
            return ViewComponent("AdminNewsAnswerCommentsItem", new { q = q, PageID = PageID });
        }

        [Route("ShowAnswer/{CommentID}")]
        public IActionResult ShowAnswer(int commentID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "دیدگاه های اخبار اپلیکیشن"))
            {
                var content = blogRepository.GetNewsAnswerByID(commentID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }


        [Route("Answer/{CommentID}")]
        public IActionResult Answer(int commentID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "دیدگاه های اخبار اپلیکیشن"))
            {
                ViewBag.CommentsID = commentID;
                ViewBag.BlogID = blogRepository.GetNewsCommentByID(commentID).NewsId;
                return View();
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Answer/{CommentID}")]
        public IActionResult Answer(NewsComment comment)
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
            if (adminRepository.CheckPermission(User.Identity.Name, "دیدگاه های اخبار اپلیکیشن"))
            {
                var content = blogRepository.GetNewsCommentByID(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Confirm/{InfoID}")]
        public IActionResult Confirm(NewsComment comment)
        {
            if (blogRepository.UpdateComment(comment))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = blogRepository.GetNewsCommentByID(comment.NewsCommentId);
                return View(content);
            }
        }

        [Route("Delete/{InfoID}")]
        public IActionResult Delete(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "دیدگاه های اخبار اپلیکیشن"))
            {
                var content = blogRepository.GetNewsCommentByID(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Delete/{InfoID}")]
        public IActionResult Delete(NewsComment comment)
        {
            if (blogRepository.Delete(comment))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = blogRepository.GetNewsCommentByID(comment.NewsCommentId);
                return View(content);
            }
        }
    }
}
