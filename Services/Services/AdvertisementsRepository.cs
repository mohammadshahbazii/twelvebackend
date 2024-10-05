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
                advertisement.Title = "Ads";
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
            List<AdvertisementsItemViewModel> model = new List<AdvertisementsItemViewModel>();
            foreach (var item in items)
            {
                model.Add(new AdvertisementsItemViewModel() 
                {
                    AdvertisementID = item.AdvertisementId,
                    ImageName = item.ImageName,
                    Link = item.Link
                });
            }
            return model;
        }

        public List<Advertisement> GetBanners()
        {
            return db.Advertisements.Where(a => a.IsBanner == true).ToList();
        }

        public Advertisement GetByID(int adsID)
        {
            return db.Advertisements.Find(adsID);
        }

        public List<Advertisement> GetLittleAds()
        {
            return db.Advertisements.Where(a => a.IsBanner == false).ToList();

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
    }
}
