using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/Features")]
    public class FeaturesController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();
        IFeaturesRepository featuresRepository = new FeaturesRepository();

        public IActionResult Index()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "ویژگی ها"))
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
            return ViewComponent("AdminFeaturesItem", new { q = q, PageID = PageID });
        }

        [Route("FeatureIntroduce")]
        public IActionResult FeatureIntroduce()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "ویژگی ها"))
            {
                var content = featuresRepository.GetFeatureIntroduces();
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
            if (adminRepository.CheckPermission(User.Identity.Name, "ویژگی ها"))
            {
                return View();
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(Feature feature, IFormFile imageProduct, IFormFile animeteFile, IFormFile imageFAProduct, IFormFile imageSAProduct , IFormFile introduceProduct)
        {
            if (featuresRepository.Create(feature, imageProduct,animeteFile,imageFAProduct,imageSAProduct,introduceProduct))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                return View();
            }
        }

        [Route("Update/{FeatureID}")]
        public IActionResult Update(int featureID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "ویژگی ها"))
            {
                var content = featuresRepository.GetByID(featureID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Update/{FeatureID}")]
        public IActionResult Update(Feature feature, IFormFile imageProduct, IFormFile animeteFile, IFormFile imageFAProduct, IFormFile imageSAProduct, IFormFile introduceProduct)
        {
            if (featuresRepository.Update(feature, imageProduct, animeteFile, imageFAProduct, imageSAProduct, introduceProduct))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = featuresRepository.GetByID(feature.FeatureId);
                return View(content);
            }
        }


        [Route("UpdateIntroduceIcon/{FeatureID}")]
        public IActionResult UpdateIntroduceIcon(int featureID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "ویژگی ها"))
            {
                var content = featuresRepository.GetByID(featureID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("UpdateIntroduceIcon/{FeatureID}")]
        public IActionResult UpdateIntroduceIcon(Feature feature, IFormFile introduceProduct)
        {
            if (featuresRepository.Update(feature,introduceProduct))
            {
                return RedirectToAction("FeatureIntroduce");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = featuresRepository.GetByID(feature.FeatureId);
                return View(content);
            }
        }

        [Route("Delete/{FeatureID}")]
        public IActionResult Delete(int featureID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "ویژگی ها"))
            {
                var content = featuresRepository.GetByID(featureID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Delete/{FeatureID}")]
        public IActionResult Delete(Feature feature)
        {
            if (featuresRepository.Delete(feature))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = featuresRepository.GetByID(feature.FeatureId);
                return View(content);
            }
        }


        #region Items
        [Route("ShowItems/{FeatureID}")]
        public IActionResult ShowItems(int featureID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "ویژگی ها"))
            {
                ViewBag.FeatureID = featureID;
                var content = featuresRepository.GetFeaturesIcons(featureID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [Route("CreateItem/{FeatureID}")]
        public IActionResult CreateItem(int FeatureID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "ویژگی ها"))
            {
                ViewBag.FeatureID = FeatureID;
                return View();
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("CreateItem/{FeatureID}")]
        public IActionResult CreateItem(FeatureItem featureItem, IFormFile imageProduct)
        {
            if (featuresRepository.CreateItem(featureItem, imageProduct))
            {
                return RedirectToAction("ShowItems", new { FeatureID = featureItem.FeatureId });
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                ViewBag.FeatureID = featureItem.FeatureId;
                return View();
            }
        }

        [Route("UpdateItem/{InfoID}")]
        public IActionResult UpdateItem(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "ویژگی ها"))
            {
                var content = featuresRepository.GetItemByID(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("UpdateItem/{InfoID}")]
        public IActionResult UpdateItem(FeatureItem featureItem, IFormFile imageProduct)
        {
            if (featuresRepository.UpdateItem(featureItem, imageProduct))
            {
                return RedirectToAction("ShowItems", new { FeatureID = featureItem.FeatureId });
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = featuresRepository.GetItemByID(featureItem.FeaturesItemId);
                return View(content);
            }
        }

        [Route("DeleteItem/{InfoID}")]
        public IActionResult DeleteItem(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "ویژگی ها"))
            {
                var content = featuresRepository.GetItemByID(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("DeleteItem/{InfoID}")]
        public IActionResult DeleteItem(FeatureItem featureItem)
        {
            if (featuresRepository.DeleteItem(featureItem))
            {
                return RedirectToAction("ShowItems", new { FeatureID = featureItem.FeatureId });
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = featuresRepository.GetItemByID(featureItem.FeaturesItemId);
                return View(content);
            }
        }
        #endregion Items
    }
}
