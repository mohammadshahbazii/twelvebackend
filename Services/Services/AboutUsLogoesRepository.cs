using DataLayer;
using Microsoft.AspNetCore.Http;
using Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AboutUsLogoesRepository : IAboutUsLogoesRepository
    {
        TwelveDbContext db = new TwelveDbContext();
        public bool Create(AboutUsLogo logo, IFormFile ImageName)
        {
            try
            {
                if (ImageName == null)
                {
                    return false;
                }
                logo.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", logo.ImageName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageName.CopyTo(stream);
                }
                db.AboutUsLogoes.Add(logo);
                db.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public bool Delete(AboutUsLogo logo)
        {
            try
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", logo.ImageName);

                System.IO.File.Delete(imagePath);
                db.AboutUsLogoes.Remove(logo);
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

        public List<AboutUsLogo> GetAboutUsLogos()
        {
            var items = db.AboutUsLogoes.ToList();
            items.ApplyTranslations(db);
            return items;
        }

        public AboutUsLogo GetByID(int id)
        {
            var item = db.AboutUsLogoes.Find(id);
            item.ApplyTranslation(db);
            return item;
        }

        public bool Update(AboutUsLogo logo, IFormFile ImageName)
        {
            try
            {
                if (ImageName != null)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", logo.ImageName);

                    System.IO.File.Delete(imagePath);

                    logo.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", logo.ImageName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        ImageName.CopyTo(stream);
                    }

                }
                db.AboutUsLogoes.Update(logo);
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
