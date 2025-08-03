using DataLayer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Services
{
    public class AdvertisementsRepository : IAdvertisementsRepository
    {
        TwelveDbContext db = new TwelveDbContext();

        public bool Create(Advertisement advertisement, IFormFile ImageName)
        {
            try
            {
                if (ImageName == null)
                {
                    return false;
                }
                advertisement.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Ads", advertisement.ImageName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageName.CopyTo(stream);
                }
                db.Advertisements.Add(advertisement);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(Advertisement advertisement)
        {
            try
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Ads", advertisement.ImageName);
                System.IO.File.Delete(imagePath);
                db.Advertisements.Remove(advertisement);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public List<AdvertisementsItemViewModel> GetAdvertisements()
        {
            var items = db.Advertisements.ToList();
            items.ApplyTranslations(db);
            List<AdvertisementsItemViewModel> model = new List<AdvertisementsItemViewModel>();
            foreach (var item in items)
            {
                model.Add(new AdvertisementsItemViewModel()
                {
                    AdvertisementID = item.AdvertisementId,
                    ImageName = item.ImageName,
                    Link = item.Link,
                    Title = item.Title
                });
            }
            return model;
        }

        public List<Advertisement> GetBanners()
        {
            var items = db.Advertisements.Where(a => a.IsBanner == true).ToList();
            items.ApplyTranslations(db);
            return items;
        }

        public Advertisement GetByID(int adsID)
        {
            var item = db.Advertisements.Find(adsID);
            item.ApplyTranslation(db);
            return item;
        }

        public AdvertisementCrudViewModel GetForEdit(int adsID)
        {
            var ad = db.Advertisements.Find(adsID);
            var model = new AdvertisementCrudViewModel
            {
                AdvertisementId = ad.AdvertisementId,
                Title = ad.Title,
                Link = ad.Link,
                IsBanner = ad.IsBanner,
                ImageName = ad.ImageName
            };
            var tr = db.EntityTranslations.ToList();
            model.TitleEn = tr.FirstOrDefault(t => t.EntityName == nameof(Advertisement) && t.EntityId == adsID && t.Property == nameof(Advertisement.Title) && t.Culture == "en")?.Value;
            model.TitleAr = tr.FirstOrDefault(t => t.EntityName == nameof(Advertisement) && t.EntityId == adsID && t.Property == nameof(Advertisement.Title) && t.Culture == "ar")?.Value;
            model.TitleUr = tr.FirstOrDefault(t => t.EntityName == nameof(Advertisement) && t.EntityId == adsID && t.Property == nameof(Advertisement.Title) && t.Culture == "ur")?.Value;
            return model;
        }

        public List<Advertisement> GetLittleAds()
        {
            var items = db.Advertisements.Where(a => a.IsBanner == false).ToList();
            items.ApplyTranslations(db);
            return items;

        }

        public bool Update(Advertisement advertisement, IFormFile ImageName)
        {
            try
            {
                if (ImageName != null)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Ads", advertisement.ImageName);

                    System.IO.File.Delete(imagePath);

                    advertisement.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Ads", advertisement.ImageName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        ImageName.CopyTo(stream);
                    }

                }
                db.Advertisements.Update(advertisement);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SaveTranslations(AdvertisementCrudViewModel advertisement)
        {
            SaveTranslation(nameof(Advertisement), advertisement.AdvertisementId, nameof(Advertisement.Title), "en", advertisement.TitleEn);
            SaveTranslation(nameof(Advertisement), advertisement.AdvertisementId, nameof(Advertisement.Title), "ar", advertisement.TitleAr);
            SaveTranslation(nameof(Advertisement), advertisement.AdvertisementId, nameof(Advertisement.Title), "ur", advertisement.TitleUr);
        }

        private void SaveTranslation(string entityName, int entityId, string property, string culture, string value)
        {
            var tr = db.EntityTranslations.FirstOrDefault(t => t.EntityName == entityName && t.EntityId == entityId && t.Property == property && t.Culture == culture);
            if (tr == null)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    db.EntityTranslations.Add(new EntityTranslation
                    {
                        EntityName = entityName,
                        EntityId = entityId,
                        Property = property,
                        Culture = culture,
                        Value = value
                    });
                    db.SaveChanges();
                }
            }
            else
            {
                tr.Value = value;
                db.EntityTranslations.Update(tr);
                db.SaveChanges();
            }
        }
    }
}
