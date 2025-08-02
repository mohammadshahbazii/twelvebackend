using DataLayer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AboutUsArticlesRepository : IAboutUsArticlesRepository
    {
        TwelveDbContext db = new TwelveDbContext();
        public bool Create(AboutUsArticle aboutUsArticle, IFormFile ImageName)
        {
            try
            {
                if (ImageName == null)
                {
                    return false;
                }
                aboutUsArticle.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", aboutUsArticle.ImageName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageName.CopyTo(stream);
                }
                db.AboutUsArticles.Add(aboutUsArticle);
                db.SaveChanges();
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

        public bool Update(AboutUsArticle aboutUsArticle, IFormFile ImageName)
        {
            try
            {
                if (ImageName != null)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", aboutUsArticle.ImageName);

                    System.IO.File.Delete(imagePath);

                    aboutUsArticle.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", aboutUsArticle.ImageName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        ImageName.CopyTo(stream);
                    }

                }
                db.AboutUsArticles.Update(aboutUsArticle);
                db.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }
    }
}
