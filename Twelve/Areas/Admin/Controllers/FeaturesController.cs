using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using ViewModels;

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
                return View(new FeatureCrudViewModel());
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(FeatureCrudViewModel feature, IFormFile imageProduct, IFormFile animeteFile, IFormFile imageFAProduct, IFormFile imageSAProduct , IFormFile introduceProduct)
        {
            if (imageProduct == null)
            {
                ViewBag.Message = "لطفا تصویر مربوطه را وارد کنید";
                return View(feature);
            }
            if (animeteFile == null)
            {
                ViewBag.Message = "لطفا فایل انیمیشن مربوطه را وارد کنید";
                return View(feature);
            }
            if (introduceProduct == null)
            {
                ViewBag.Message = "لطفا تصویر معرفی نرم افزار در صفحه اول مربوطه را وارد کنید";
                return View(feature);
            }
            var entity = new Feature
            {
                Title = feature.Title,
                ShortDescription = feature.ShortDescription,
                FirstDescription = feature.FirstDescription,
                FirstArticleTitle = feature.FirstArticleTitle,
                FirstArticleDescription = feature.FirstArticleDescription,
                SecondArticleTitle = feature.SecondArticleTitle,
                SecondArticleDescription = feature.SecondArticleDescription
            };
            if (featuresRepository.Create(entity, imageProduct, animeteFile, imageFAProduct, imageSAProduct, introduceProduct))
            {
                feature.FeatureId = entity.FeatureId;
                featuresRepository.SaveTranslations(feature);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                return View(feature);
            }
        }

        [Route("Update/{FeatureID}")]
        public IActionResult Update(int featureID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "ویژگی ها"))
            {
                var content = featuresRepository.GetForEdit(featureID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Update/{FeatureID}")]
        public IActionResult Update(FeatureCrudViewModel feature, IFormFile imageProduct, IFormFile animeteFile, IFormFile imageFAProduct, IFormFile imageSAProduct, IFormFile introduceProduct)
        {
            var entity = new Feature
            {
                FeatureId = feature.FeatureId,
                Title = feature.Title,
                ShortDescription = feature.ShortDescription,
                FirstDescription = feature.FirstDescription,
                FirstArticleTitle = feature.FirstArticleTitle,
                FirstArticleDescription = feature.FirstArticleDescription,
                SecondArticleTitle = feature.SecondArticleTitle,
                SecondArticleDescription = feature.SecondArticleDescription,
                ImageName = feature.ImageName,
                AnimateFilename = feature.AnimateFilename,
                FirstArticleImage = feature.FirstArticleImage,
                SecondArticleImage = feature.SecondArticleImage,
                IntroduceImageName = feature.IntroduceImageName
            };
            if (featuresRepository.Update(entity, imageProduct, animeteFile, imageFAProduct, imageSAProduct, introduceProduct))
            {
                featuresRepository.SaveTranslations(feature);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = featuresRepository.GetForEdit(feature.FeatureId);
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
                return View(new FeatureItemCrudViewModel { FeatureId = FeatureID });
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("CreateItem/{FeatureID}")]
        public IActionResult CreateItem(FeatureItemCrudViewModel featureItem, IFormFile imageProduct)
        {
            var entity = new FeatureItem
            {
                FeatureId = featureItem.FeatureId,
                Title = featureItem.Title,
                ShortDescription = featureItem.ShortDescription
            };
            if (featuresRepository.CreateItem(entity, imageProduct))
            {
                featureItem.FeaturesItemId = entity.FeaturesItemId;
                featuresRepository.SaveItemTranslations(featureItem);
                return RedirectToAction("ShowItems", new { FeatureID = featureItem.FeatureId });
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                ViewBag.FeatureID = featureItem.FeatureId;
                return View(featureItem);
            }
        }

        [Route("UpdateItem/{InfoID}")]
        public IActionResult UpdateItem(int InfoID)
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "ویژگی ها"))
            {
                var content = featuresRepository.GetItemForEdit(InfoID);
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [Route("UpdateItem/{InfoID}")]
        public IActionResult UpdateItem(FeatureItemCrudViewModel featureItem, IFormFile imageProduct)
        {
            var entity = new FeatureItem
            {
                FeaturesItemId = featureItem.FeaturesItemId,
                FeatureId = featureItem.FeatureId,
                Title = featureItem.Title,
                ShortDescription = featureItem.ShortDescription,
                ImageName = featureItem.ImageName
            };
            if (featuresRepository.UpdateItem(entity, imageProduct))
            {
                featuresRepository.SaveItemTranslations(featureItem);
                return RedirectToAction("ShowItems", new { FeatureID = featureItem.FeatureId });
            }
            else
            {
                ViewBag.Message = "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش کنید";
                var content = featuresRepository.GetItemForEdit(featureItem.FeaturesItemId);
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
