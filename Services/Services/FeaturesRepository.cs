using DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using ViewModels;

namespace Services
{
    public class FeaturesRepository : IFeaturesRepository
    {
        TwelveDbContext db = new TwelveDbContext();

        public void Dispose()
        {
            db.Dispose();
        }

        public AdminFeaturesPageDataViewModel GetAdminFeatures(string q = "", int PageID = 1)
        {
            AdminFeaturesPageDataViewModel model = new AdminFeaturesPageDataViewModel();
            int take = 8;
            int skip = (PageID - 1) * take;
            model.Features = new List<FeaturesItemViewModel>();
            if (string.IsNullOrEmpty(q))
            {
                var features = db.Features.Skip(skip).Take(take).ToList();
                features.ApplyTranslations(db);
                foreach (var feature in features)
                {
                    model.Features.Add(new FeaturesItemViewModel()
                    {
                        FeatureID = feature.FeatureId,
                        Title = feature.Title,
                        ImageName = feature.ImageName,
                        ShortDescription = feature.ShortDescription,
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.Features.ToList().Count()) / Convert.ToDouble(take));
                model.PageCount = pCount;
                model.Pagination = GetPagination(pCount, PageID);
                return model;
            }
            else
            {
                var features = db.Features.Where(f => f.Title.Contains(q)).Skip(skip).Take(take).ToList();
                features.ApplyTranslations(db);
                foreach (var feature in features)
                {
                    model.Features.Add(new FeaturesItemViewModel()
                    {
                        FeatureID = feature.FeatureId,
                        Title = feature.Title,
                        ImageName = feature.ImageName,
                        ShortDescription = feature.ShortDescription,
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.Features.Where(f => f.Title.Contains(q)).ToList().Count()) / Convert.ToDouble(take));
                model.PageCount = pCount;
                model.Pagination = GetPagination(pCount, PageID);
                return model;
            }
        }

        public PaginationViewModel GetPagination(double PageCount, int PageID = 1)
        {
            PaginationViewModel pagination = new PaginationViewModel();
            pagination.PageNumbers = new List<int>();
            int page = Convert.ToInt32(PageCount) + 1;
            if (PageID < 3)
            {
                for (int i = 1; i < 6; i++)
                {
                    if (i < page)
                    {
                        pagination.PageNumbers.Add(i);
                    }
                }
            }
            else
            {
                for (int i = PageID - 2; i <= PageID + 2; i++)
                {

                    if (i < page)
                    {
                        pagination.PageNumbers.Add(i);
                    }
                }
            }

            pagination.PageCount = Convert.ToInt32(PageCount);
            pagination.PageID = PageID;

            return pagination;
        }

        public FeaturePageDataViewModel GetFeature(string Demo)
        {
            string title = Demo.Replace("-"," ");
            Feature feature = db.Features.FirstOrDefault(f => f.Title == title);
            feature.ApplyTranslation(db);
            FeaturePageDataViewModel model = new FeaturePageDataViewModel()
            {
                FeatureID = feature.FeatureId,
                Title = title,
                ShortDescription = feature.FirstDescription,
                FirstNewsTitle = feature.FirstArticleTitle,
                FirstNewsImageName = feature.FirstArticleImage,
                FirstNewsShortDescription = feature.FirstArticleDescription,
                SecondNewsTitle = feature.SecondArticleTitle,
                SecondNewsShortDescription = feature.SecondArticleDescription,
                SecondNewsImageName = feature.SecondArticleImage,
            };
            model.Icons = new List<FeatureIconsViewModel>();
            var items = db.FeatureItems.Where(f => f.FeatureId == feature.FeatureId).ToList();
            items.ApplyTranslations(db);
            foreach ( var item in items)
            {
                model.Icons.Add(new FeatureIconsViewModel() 
                {
                    ItemID = item.FeaturesItemId,
                    Title = item.Title,
                    Description = item.ShortDescription,
                    ImageName = item.ImageName
                });
            }
            return model;
        }

        public List<FeaturesItemViewModel> GetFeatures()
        {
            var content = db.Features.ToList();
            content.ApplyTranslations(db);
            List<FeaturesItemViewModel> model = new List<FeaturesItemViewModel>();
            for (int i=0;i<content.Count;i++)
            {
                model.Add(new FeaturesItemViewModel() 
                {
                    FeatureID = content[i].FeatureId,
                    ImageName = content[i].ImageName,
                    AnimateImage = content[i].AnimateFilename,
                    ShortDescription = content[i].ShortDescription,
                    Title = content[i].Title,
                    Demo = content[i].Title.Replace(" ","-"),
                    IsBig = BigFeatureChecker.Check(i+1)
                    
                });
            }
            return model;
        }

        public List<FeatureIconsViewModel> GetFeaturesIcons(int featureID)
        {
            var Icons = new List<FeatureIconsViewModel>();
            var items = db.FeatureItems.Where(f => f.FeatureId == featureID).ToList();
            items.ApplyTranslations(db);
            foreach (var item in items)
            {
                Icons.Add(new FeatureIconsViewModel()
                {
                    ItemID = item.FeaturesItemId,
                    Title = item.Title,
                    Description = item.ShortDescription,
                    ImageName = item.ImageName
                });
            }
            return Icons;
        }

        public FeatureItem GetItemByID(int ItemID)
        {
            var item = db.FeatureItems.Find(ItemID);
            item.ApplyTranslation(db);
            return item;
        }

        public bool CreateItem(FeatureItem featureItem, IFormFile ImageName)
        {
            try
            {
                if (ImageName == null)
                {
                    return false;
                }
                featureItem.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features", featureItem.ImageName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageName.CopyTo(stream);
                }
                db.FeatureItems.Add(featureItem);
                db.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public bool UpdateItem(FeatureItem featureItem, IFormFile ImageName)
        {
            try
            {
                if (ImageName != null)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features", featureItem.ImageName);

                    System.IO.File.Delete(imagePath);

                    featureItem.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features", featureItem.ImageName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        ImageName.CopyTo(stream);
                    }

                }
                db.FeatureItems.Update(featureItem);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool DeleteItem(FeatureItem featureItem)
        {
            try
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features", featureItem.ImageName);

                System.IO.File.Delete(imagePath);

                db.FeatureItems.Remove(featureItem);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Create(Feature feature, IFormFile ImageName, IFormFile AnimateFile, IFormFile FirstArticleImage, IFormFile SecondArticleImage ,  IFormFile IntroduceImage)
        {
            try
            {
                if (ImageName == null || AnimateFile == null || FirstArticleImage == null || SecondArticleImage == null || IntroduceImage == null)
                {
                    return false;
                }
                feature.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features", feature.ImageName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageName.CopyTo(stream);
                }

                feature.AnimateFilename = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(AnimateFile.FileName);
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features/Json", feature.AnimateFilename);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    AnimateFile.CopyTo(stream);
                }

                feature.FirstArticleImage = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(FirstArticleImage.FileName);
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features", feature.FirstArticleImage);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    FirstArticleImage.CopyTo(stream);
                }

                feature.SecondArticleImage = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(SecondArticleImage.FileName);
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features", feature.SecondArticleImage);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    SecondArticleImage.CopyTo(stream);
                }

                feature.IntroduceImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(IntroduceImage.FileName);
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features", feature.IntroduceImageName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    IntroduceImage.CopyTo(stream);
                }

                db.Features.Add(feature);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public Feature GetByID(int id)
        {
            return db.Features.Find(id);
        }

        public bool Update(Feature feature, IFormFile ImageName, IFormFile AnimateFile, IFormFile FirstArticleImage, IFormFile SecondArticleImage, IFormFile IntroduceImage)
        {
            try
            {
                if (ImageName != null)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features", feature.ImageName);

                    System.IO.File.Delete(imagePath);

                    feature.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features", feature.ImageName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        ImageName.CopyTo(stream);
                    }

                }

                if (AnimateFile != null)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features/Json", feature.AnimateFilename);

                    System.IO.File.Delete(imagePath);

                    feature.AnimateFilename = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(AnimateFile.FileName);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features/Json", feature.AnimateFilename);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        AnimateFile.CopyTo(stream);
                    }

                }

                if (FirstArticleImage != null)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features", feature.FirstArticleImage);

                    System.IO.File.Delete(imagePath);

                    feature.FirstArticleImage = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(FirstArticleImage.FileName);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features", feature.FirstArticleImage);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        FirstArticleImage.CopyTo(stream);
                    }

                }

                if (SecondArticleImage != null)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features", feature.SecondArticleImage);

                    System.IO.File.Delete(imagePath);

                    feature.SecondArticleImage = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(SecondArticleImage.FileName);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features", feature.SecondArticleImage);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        SecondArticleImage.CopyTo(stream);
                    }

                }
                if (IntroduceImage != null)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features", feature.IntroduceImageName);

                    System.IO.File.Delete(imagePath);

                    feature.IntroduceImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(IntroduceImage.FileName);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features", feature.IntroduceImageName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        IntroduceImage.CopyTo(stream);
                    }

                }
                db.Features.Update(feature);
                db.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public bool Update(Feature feature, IFormFile IntroduceImage)
        {
            try
            {
                if (IntroduceImage != null)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features", feature.IntroduceImageName);

                    System.IO.File.Delete(imagePath);

                    feature.IntroduceImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(IntroduceImage.FileName);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features", feature.IntroduceImageName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        IntroduceImage.CopyTo(stream);
                    }

                }
                db.Features.Update(feature);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(Feature feature)
        {
            try
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features", feature.SecondArticleImage);

                System.IO.File.Delete(imagePath);

                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features", feature.FirstArticleImage);

                System.IO.File.Delete(imagePath);

                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features/Json", feature.AnimateFilename);

                System.IO.File.Delete(imagePath);

                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features", feature.ImageName);

                System.IO.File.Delete(imagePath);

                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Features", feature.IntroduceImageName);

                System.IO.File.Delete(imagePath);

                db.FeatureItems.Where(f => f.FeatureId == feature.FeatureId).ToList().ForEach(f => db.FeatureItems.Remove(f));
                db.SaveChanges() ;

                db.SelectedFeatureNews.Where(f => f.FeatureId == feature.FeatureId).ToList().ForEach(f => db.SelectedFeatureNews.Remove(f));
                db.SaveChanges();

                db.Features.Remove(feature);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public List<BlogItemViewModel> GetRelatedNews(int FeatureID)
        {
            List<BlogItemViewModel> model = new List<BlogItemViewModel>();
            var items = db.SelectedFeatureNews.Where(f => f.FeatureId == FeatureID).Select(f => f.News).ToList();
            foreach ( var item in items)
            {
                model.Add(new BlogItemViewModel() 
                {
                    BlogID = item.NewsId,
                    CreateDate = DateConvertor.PassDays(item.CreateDate),
                    Title = item.Title,
                    ImageName = item.ImageName,
                    Source = item.Source
                });
            }
            return model;
        }

        public IntroduceAppViewModel GetIntroduceApp()
        {
            var features = db.Introduces.Select(i => i.Feature).Distinct().ToList();
            IntroduceAppViewModel model = new IntroduceAppViewModel();
            model.items = new List<IntroduceAppItemViewModel>();
            foreach ( var item in features)
            {
                var items = db.Introduces.Where(i => i.FeatureId == item.FeatureId).ToList();
                List<ContentItemViewModel> contents = new List<ContentItemViewModel>();
                foreach (var content in items)
                {
                    contents.Add(new ContentItemViewModel() 
                    {
                        ID = item.FeatureId,
                        Description = content.IntroduceText,
                        Title = content.IntroduceTitle,
                    }) ;
                }
                model.items.Add(new IntroduceAppItemViewModel() 
                {
                    Features = new FeatureNameViewModel() 
                    { 
                        FeatureID = item.FeatureId ,
                        ImageName = item.IntroduceImageName ,
                        Title = item.Title
                    },
                    FeatureItems = contents
                });
                model.Sliders = db.Sliders.Where(s => s.SliderGroupId == 7).Select(s => s.ImageName).ToList();
                
            }
            return model;
        }

        public FeaturesContent GetFeaturesContent()
        {
            return db.FeaturesContents.FirstOrDefault();
        }

        public bool UpdateFeaturesContent(FeaturesContent content)
        {
            try
            {
                db.FeaturesContents.Update(content);
                db.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public List<Feature> GetFeatureIntroduces()
        {
            return db.Features.ToList();
        }

        
    }
}
