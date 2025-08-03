using DataLayer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Services
{
    public class AboutUsItemsRepository : IAboutUsItemsRepository
    {
        TwelveDbContext db = new TwelveDbContext();
        public bool Create(AboutUsItemCrudViewModel item, IFormFile ImageName)
        {
            try
            {
                if (ImageName == null)
                {
                    return false;
                }
                var entity = new AboutUsItem
                {
                    ItemTitle = item.ItemTitle,
                    ItemDescription = item.ItemDescription
                };
                entity.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", entity.ImageName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageName.CopyTo(stream);
                }
                db.AboutUsItems.Add(entity);
                db.SaveChanges();
                item.AboutUsItemId = entity.AboutUsItemId;
                item.ImageName = entity.ImageName;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(AboutUsItem item)
        {
            try
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", item.ImageName);

                System.IO.File.Delete(imagePath);
                db.AboutUsItems.Remove(item);
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

        public List<AboutUsItem> GetAboutUsItems()
        {
            var items = db.AboutUsItems.ToList();
            items.ApplyTranslations(db);
            return items;
        }

        public AboutUsItem GetByID(int itemID)
        {
            var item = db.AboutUsItems.Find(itemID);
            item.ApplyTranslation(db);
            return item;
        }

        public AboutUsItemCrudViewModel GetForEdit(int itemID)
        {
            var item = db.AboutUsItems.Find(itemID);
            var translations = db.EntityTranslations.Where(t => t.EntityName == nameof(AboutUsItem) && t.EntityId == itemID).ToList();
            return new AboutUsItemCrudViewModel
            {
                AboutUsItemId = item.AboutUsItemId,
                ItemTitle = item.ItemTitle,
                ItemDescription = item.ItemDescription,
                ImageName = item.ImageName,
                ItemTitleEn = translations.FirstOrDefault(t => t.Property == nameof(AboutUsItem.ItemTitle) && t.Culture == "en")?.Value,
                ItemTitleAr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsItem.ItemTitle) && t.Culture == "ar")?.Value,
                ItemTitleUr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsItem.ItemTitle) && t.Culture == "ur")?.Value,
                ItemDescriptionEn = translations.FirstOrDefault(t => t.Property == nameof(AboutUsItem.ItemDescription) && t.Culture == "en")?.Value,
                ItemDescriptionAr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsItem.ItemDescription) && t.Culture == "ar")?.Value,
                ItemDescriptionUr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsItem.ItemDescription) && t.Culture == "ur")?.Value
            };
        }

        public bool Update(AboutUsItemCrudViewModel item, IFormFile ImageName)
        {
            try
            {
                var entity = db.AboutUsItems.Find(item.AboutUsItemId);
                entity.ItemTitle = item.ItemTitle;
                entity.ItemDescription = item.ItemDescription;
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
                db.AboutUsItems.Update(entity);
                db.SaveChanges();
                item.ImageName = entity.ImageName;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SaveTranslations(AboutUsItemCrudViewModel item)
        {
            SaveTranslation(nameof(AboutUsItem), item.AboutUsItemId, nameof(AboutUsItem.ItemTitle), "en", item.ItemTitleEn);
            SaveTranslation(nameof(AboutUsItem), item.AboutUsItemId, nameof(AboutUsItem.ItemTitle), "ar", item.ItemTitleAr);
            SaveTranslation(nameof(AboutUsItem), item.AboutUsItemId, nameof(AboutUsItem.ItemTitle), "ur", item.ItemTitleUr);
            SaveTranslation(nameof(AboutUsItem), item.AboutUsItemId, nameof(AboutUsItem.ItemDescription), "en", item.ItemDescriptionEn);
            SaveTranslation(nameof(AboutUsItem), item.AboutUsItemId, nameof(AboutUsItem.ItemDescription), "ar", item.ItemDescriptionAr);
            SaveTranslation(nameof(AboutUsItem), item.AboutUsItemId, nameof(AboutUsItem.ItemDescription), "ur", item.ItemDescriptionUr);
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
