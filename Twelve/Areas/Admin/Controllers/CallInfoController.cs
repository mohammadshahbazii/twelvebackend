using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using ViewModels;

namespace Twelve.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/CallInfo")]
    public class CallInfoController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();
        ICallInfoRepository callInfoRepository = new CallInfoRepository();
        public IActionResult Index()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "اطلاعات تماس"))
            {
                var content = callInfoRepository.GetCallInfoes();
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
            if (adminRepository.CheckPermission(User.Identity.Name, "اطلاعات تماس"))
            {
                return View(new CallInfoCrudViewModel());
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(CallInfoCrudViewModel callInfo , IFormFile imageProduct)
        {
            var entity = new CallInfo
            {
                Title = callInfo.Title,
                ShortDescription = callInfo.ShortDescription,
                Description = callInfo.Description
            };
            if (callInfoRepository.Create(entity,imageProduct))
            {
                callInfo.CallInfoId = entity.CallInfoId;
                callInfoRepository.SaveTranslations(callInfo);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                return View(callInfo);
            }
        }

        [Route("Update/{InfoID}")]
        public IActionResult Update(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "اطلاعات تماس"))
            {
                var content = callInfoRepository.GetForEdit(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Update/{InfoID}")]
        public IActionResult Update(CallInfoCrudViewModel callInfo, IFormFile imageProduct)
        {
            var entity = new CallInfo
            {
                CallInfoId = callInfo.CallInfoId,
                Title = callInfo.Title,
                ShortDescription = callInfo.ShortDescription,
                Description = callInfo.Description,
                ImageName = callInfo.ImageName
            };
            if (callInfoRepository.Update(entity, imageProduct))
            {
                callInfoRepository.SaveTranslations(callInfo);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = callInfoRepository.GetForEdit(callInfo.CallInfoId);
                return View(content);
            }
        }

        [Route("Delete/{InfoID}")]
        public IActionResult Delete(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "اطلاعات تماس"))
            {
                var content = callInfoRepository.GetByID(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Delete/{InfoID}")]
        public IActionResult Delete(CallInfo callInfo)
        {
            if (callInfoRepository.Delete(callInfo))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = callInfoRepository.GetByID(callInfo.CallInfoId);
                return View(content);
            }
        }
    }
}
