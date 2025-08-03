using DataLayer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Services
{
    public class AboutUsSightRepository : IAboutUsSightRepository
    {
        TwelveDbContext db = new TwelveDbContext();

        public bool Create(AboutUsSightCrudViewModel sight, IFormFile ImageName)
        {
            try
            {
                if (ImageName == null)
                {
                    return false;
                }
                var entity = new AboutUsSight
                {
                    SightTitle = sight.SightTitle,
                    SightDescription = sight.SightDescription
                };
                entity.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", entity.ImageName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageName.CopyTo(stream);
                }
                db.AboutUsSights.Add(entity);
                db.SaveChanges();
                sight.AboutUsSightId = entity.AboutUsSightId;
                sight.ImageName = entity.ImageName;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(AboutUsSight sight)
        {
            try
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", sight.ImageName);

                System.IO.File.Delete(imagePath);
                db.AboutUsSights.Remove(sight);
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

        public List<AboutUsSight> GetAboutUsSights()
        {
            var items = db.AboutUsSights.ToList();
            items.ApplyTranslations(db);
            return items;
        }

        public AboutUsSight GetByID(int aboutUsSightID)
        {
            var item = db.AboutUsSights.Find(aboutUsSightID);
            item.ApplyTranslation(db);
            return item;
        }

        public AboutUsSightCrudViewModel GetForEdit(int aboutUsSightID)
        {
            var item = db.AboutUsSights.Find(aboutUsSightID);
            var translations = db.EntityTranslations.Where(t => t.EntityName == nameof(AboutUsSight) && t.EntityId == aboutUsSightID).ToList();
            return new AboutUsSightCrudViewModel
            {
                AboutUsSightId = item.AboutUsSightId,
                SightTitle = item.SightTitle,
                SightDescription = item.SightDescription,
                ImageName = item.ImageName,
                SightTitleEn = translations.FirstOrDefault(t => t.Property == nameof(AboutUsSight.SightTitle) && t.Culture == "en")?.Value,
                SightTitleAr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsSight.SightTitle) && t.Culture == "ar")?.Value,
                SightTitleUr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsSight.SightTitle) && t.Culture == "ur")?.Value,
                SightDescriptionEn = translations.FirstOrDefault(t => t.Property == nameof(AboutUsSight.SightDescription) && t.Culture == "en")?.Value,
                SightDescriptionAr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsSight.SightDescription) && t.Culture == "ar")?.Value,
                SightDescriptionUr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsSight.SightDescription) && t.Culture == "ur")?.Value
            };
        }

        public bool Update(AboutUsSightCrudViewModel sight, IFormFile ImageName)
        {
            try
            {
                var entity = db.AboutUsSights.Find(sight.AboutUsSightId);
                entity.SightTitle = sight.SightTitle;
                entity.SightDescription = sight.SightDescription;
                if (ImageName != null)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", entity.ImageName);
                    System.IO.File.Delete(imagePath);
                    entity.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", entity.ImageName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        ImageName.CopyTo(stream);
                    }
                }
                db.AboutUsSights.Update(entity);
                db.SaveChanges();
                sight.ImageName = entity.ImageName;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SaveTranslations(AboutUsSightCrudViewModel sight)
        {
            SaveTranslation(nameof(AboutUsSight), sight.AboutUsSightId, nameof(AboutUsSight.SightTitle), "en", sight.SightTitleEn);
            SaveTranslation(nameof(AboutUsSight), sight.AboutUsSightId, nameof(AboutUsSight.SightTitle), "ar", sight.SightTitleAr);
            SaveTranslation(nameof(AboutUsSight), sight.AboutUsSightId, nameof(AboutUsSight.SightTitle), "ur", sight.SightTitleUr);
            SaveTranslation(nameof(AboutUsSight), sight.AboutUsSightId, nameof(AboutUsSight.SightDescription), "en", sight.SightDescriptionEn);
            SaveTranslation(nameof(AboutUsSight), sight.AboutUsSightId, nameof(AboutUsSight.SightDescription), "ar", sight.SightDescriptionAr);
            SaveTranslation(nameof(AboutUsSight), sight.AboutUsSightId, nameof(AboutUsSight.SightDescription), "ur", sight.SightDescriptionUr);
        }

        private void SaveTranslation(string entityName, int entityId, string property, string culture, string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            var tr = db.EntityTranslations.FirstOrDefault(t => t.EntityName == entityName && t.EntityId == entityId && t.Property == property && t.Culture == culture);
            if (tr == null)
            {
                tr = new EntityTranslation
                {
                    EntityName = entityName,
                    EntityId = entityId,
                    Property = property,
                    Culture = culture
                };
                db.EntityTranslations.Add(tr);
            }
            tr.Value = value;
            db.SaveChanges();
        }
    }
}
