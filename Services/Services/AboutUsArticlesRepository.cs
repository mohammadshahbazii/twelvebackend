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
    public class AboutUsArticlesRepository : IAboutUsArticlesRepository
    {
        TwelveDbContext db = new TwelveDbContext();
        public bool Create(AboutUsArticleCrudViewModel aboutUsArticle, IFormFile ImageName)
        {
            try
            {
                if (ImageName == null)
                {
                    return false;
                }
                var entity = new AboutUsArticle
                {
                    Title = aboutUsArticle.Title,
                    SubTitle = aboutUsArticle.SubTitle,
                    Description = aboutUsArticle.Description
                };
                entity.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", entity.ImageName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageName.CopyTo(stream);
                }
                db.AboutUsArticles.Add(entity);
                db.SaveChanges();
                aboutUsArticle.AboutUsArticleId = entity.AboutUsArticleId;
                aboutUsArticle.ImageName = entity.ImageName;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(AboutUsArticle aboutUsArticle)
        {
            try
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", aboutUsArticle.ImageName);

                System.IO.File.Delete(imagePath);
                db.AboutUsArticles.Remove(aboutUsArticle);
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

        public List<AboutUsArticle> GetAboutUsArticles()
        {
            var items = db.AboutUsArticles.ToList();
            items.ApplyTranslations(db);
            return items;
        }

        public AboutUsArticle GetByID(int aboutUsArticleID)
        {
            var item = db.AboutUsArticles.Find(aboutUsArticleID);
            item.ApplyTranslation(db);
            return item;
        }

        public AboutUsArticleCrudViewModel GetForEdit(int aboutUsArticleID)
        {
            var item = db.AboutUsArticles.Find(aboutUsArticleID);
            var translations = db.EntityTranslations.Where(t => t.EntityName == nameof(AboutUsArticle) && t.EntityId == aboutUsArticleID).ToList();
            return new AboutUsArticleCrudViewModel
            {
                AboutUsArticleId = item.AboutUsArticleId,
                Title = item.Title,
                SubTitle = item.SubTitle,
                Description = item.Description,
                ImageName = item.ImageName,
                TitleEn = translations.FirstOrDefault(t => t.Property == nameof(AboutUsArticle.Title) && t.Culture == "en")?.Value,
                TitleAr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsArticle.Title) && t.Culture == "ar")?.Value,
                TitleUr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsArticle.Title) && t.Culture == "ur")?.Value,
                SubTitleEn = translations.FirstOrDefault(t => t.Property == nameof(AboutUsArticle.SubTitle) && t.Culture == "en")?.Value,
                SubTitleAr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsArticle.SubTitle) && t.Culture == "ar")?.Value,
                SubTitleUr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsArticle.SubTitle) && t.Culture == "ur")?.Value,
                DescriptionEn = translations.FirstOrDefault(t => t.Property == nameof(AboutUsArticle.Description) && t.Culture == "en")?.Value,
                DescriptionAr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsArticle.Description) && t.Culture == "ar")?.Value,
                DescriptionUr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsArticle.Description) && t.Culture == "ur")?.Value
            };
        }

        public bool Update(AboutUsArticleCrudViewModel aboutUsArticle, IFormFile ImageName)
        {
            try
            {
                var entity = db.AboutUsArticles.Find(aboutUsArticle.AboutUsArticleId);
                entity.Title = aboutUsArticle.Title;
                entity.SubTitle = aboutUsArticle.SubTitle;
                entity.Description = aboutUsArticle.Description;
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
                db.AboutUsArticles.Update(entity);
                db.SaveChanges();
                aboutUsArticle.ImageName = entity.ImageName;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SaveTranslations(AboutUsArticleCrudViewModel aboutUsArticle)
        {
            SaveTranslation(nameof(AboutUsArticle), aboutUsArticle.AboutUsArticleId, nameof(AboutUsArticle.Title), "en", aboutUsArticle.TitleEn);
            SaveTranslation(nameof(AboutUsArticle), aboutUsArticle.AboutUsArticleId, nameof(AboutUsArticle.Title), "ar", aboutUsArticle.TitleAr);
            SaveTranslation(nameof(AboutUsArticle), aboutUsArticle.AboutUsArticleId, nameof(AboutUsArticle.Title), "ur", aboutUsArticle.TitleUr);
            SaveTranslation(nameof(AboutUsArticle), aboutUsArticle.AboutUsArticleId, nameof(AboutUsArticle.SubTitle), "en", aboutUsArticle.SubTitleEn);
            SaveTranslation(nameof(AboutUsArticle), aboutUsArticle.AboutUsArticleId, nameof(AboutUsArticle.SubTitle), "ar", aboutUsArticle.SubTitleAr);
            SaveTranslation(nameof(AboutUsArticle), aboutUsArticle.AboutUsArticleId, nameof(AboutUsArticle.SubTitle), "ur", aboutUsArticle.SubTitleUr);
            SaveTranslation(nameof(AboutUsArticle), aboutUsArticle.AboutUsArticleId, nameof(AboutUsArticle.Description), "en", aboutUsArticle.DescriptionEn);
            SaveTranslation(nameof(AboutUsArticle), aboutUsArticle.AboutUsArticleId, nameof(AboutUsArticle.Description), "ar", aboutUsArticle.DescriptionAr);
            SaveTranslation(nameof(AboutUsArticle), aboutUsArticle.AboutUsArticleId, nameof(AboutUsArticle.Description), "ur", aboutUsArticle.DescriptionUr);
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
