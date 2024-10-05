using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Dynamic;

namespace Twelve.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/CallInfoLinks")]
    public class CallInfoLinksController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();
        ICallInfoLinksRepository callInfoLinksRepository = new CallInfoLinksRepository();

        [Route("Links/{CallInfoID}")]
        public IActionResult Links(int callInfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "اطلاعات تماس"))
            {
                var content = callInfoLinksRepository.GetLinks(callInfoID);
                ViewBag.CallInfoID = callInfoID;
                return View(content);
            }
            else
            {
                return View("Error");
            }
            
        }

        [Route("Create/{CallInfoID}")]
        public IActionResult Create(int callInfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "اطلاعات تماس"))
            {
                ViewBag.CallInfoID = callInfoID;
                return View();
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Create/{CallInfoID}")]
        public IActionResult Create(CallInfoLink callInfoLink)
        {
            if (callInfoLinksRepository.Create(callInfoLink))
            {
                return RedirectToAction("Links",new { CallInfoID = callInfoLink.CallInfoId });
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                ViewBag.CallInfoID = callInfoLink.CallInfoId;
                return View();
            }
        }



        [Route("Update/{CallInfoLinkID}")]
        public IActionResult Update(int callInfoLinkID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "اطلاعات تماس"))
            {
                var content = callInfoLinksRepository.GetByID(callInfoLinkID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Update/{CallInfoLinkID}")]
        public IActionResult Update(CallInfoLink callInfoLink)
        {
            if (callInfoLinksRepository.Update(callInfoLink))
            {
                return RedirectToAction("Links", new { CallInfoID = callInfoLink.CallInfoId });
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = callInfoLinksRepository.GetByID(callInfoLink.CallInfoLinkId);
                return View(content);
            }
        }

        [Route("Delete/{CallInfoLinkID}")]
        public IActionResult Delete(int callInfoLinkID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "اطلاعات تماس"))
            {
                var content = callInfoLinksRepository.GetByID(callInfoLinkID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Delete/{CallInfoLinkID}")]
        public IActionResult Delete(CallInfoLink callInfoLink)
        {
            if (callInfoLinksRepository.Delete(callInfoLink))
            {
                return RedirectToAction("Links", new { CallInfoID = callInfoLink.CallInfoId });
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = callInfoLinksRepository.GetByID(callInfoLink.CallInfoLinkId);
                return View(content);
            }
        }
    }
}
