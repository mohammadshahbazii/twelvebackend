using DataLayer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AboutUsSightRepository : IAboutUsSightRepository
    {
        TwelveDbContext db = new TwelveDbContext();

        public bool Create(AboutUsSight sight , IFormFile ImageName)
        {
            try
            {
                if (ImageName == null)
                {
                    return false;
                }
                sight.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", sight.ImageName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageName.CopyTo(stream);
                }
                db.AboutUsSights.Add(sight);
                db.SaveChanges();
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

        public bool Update(AboutUsSight sight , IFormFile ImageName)
        {
            try
            {
                if (ImageName != null)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", sight.ImageName);

                    System.IO.File.Delete(imagePath);

                    sight.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", sight.ImageName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        ImageName.CopyTo(stream);
                    }

                }
                db.AboutUsSights.Update(sight);
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
