using DataLayer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AboutUsItemsRepository : IAboutUsItemsRepository
    {
        TwelveDbContext db = new TwelveDbContext();
        public bool Create(AboutUsItem item, IFormFile ImageName)
        {
            try
            {
                if (ImageName == null)
                {
                    return false;
                }
                item.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", item.ImageName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageName.CopyTo(stream);
                }
                db.AboutUsItems.Add(item);
                db.SaveChanges();
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

        public bool Update(AboutUsItem item, IFormFile ImageName)
        {
            try
            {
                if (ImageName != null)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", item.ImageName);

                    System.IO.File.Delete(imagePath);

                    item.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", item.ImageName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        ImageName.CopyTo(stream);
                    }

                }
                db.AboutUsItems.Update(item);
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
