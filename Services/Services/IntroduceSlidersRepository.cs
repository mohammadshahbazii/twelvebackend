using DataLayer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class IntroduceSlidersRepository : IIntroduceSlidersRepository
    {
        TwelveDbContext db = new TwelveDbContext();
        public bool Create(IntroduceSlider slider, IFormFile ImageName)
        {
            try
            {
                if (ImageName == null)
                {
                    return false;
                }
                slider.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Sliders", slider.ImageName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageName.CopyTo(stream);
                }
                slider.Title = "دوازده";
                db.IntroduceSliders.Add(slider);
                db.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public bool Delete(IntroduceSlider slider)
        {
            try
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Sliders", slider.ImageName);
                System.IO.File.Delete(imagePath);
                db.IntroduceSliders.Remove(slider);
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

        public IntroduceSlider GetByID(int id)
        {
            return db.IntroduceSliders.Find(id);
        }

        public List<IntroduceSlider> GetListByFeatureID(int featureID)
        {
            return db.IntroduceSliders.Where(s => s.FeatureId == featureID).ToList();
        }

        public bool Update(IntroduceSlider slider, IFormFile ImageName)
        {
            try
            {
                if (ImageName != null)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Sliders", slider.ImageName);

                    System.IO.File.Delete(imagePath);

                    slider.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Sliders", slider.ImageName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        ImageName.CopyTo(stream);
                    }

                }
                db.IntroduceSliders.Update(slider);
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
