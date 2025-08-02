using DataLayer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using ViewModels;

namespace Services
{
    public class SlidersRepository : ISlidersRepository
    {
        TwelveDbContext db = new TwelveDbContext();

        public bool Create(Slider slider, IFormFile ImageName)
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
                db.Sliders.Add(slider);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool Delete(Slider slider)
        {
            try
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Sliders", slider.ImageName);
                System.IO.File.Delete(imagePath);
                db.Sliders.Remove(slider);
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

        public List<IndexSliderItemViewModel> GetAboutUsSlider()
        {
            var content = db.Sliders.Where(s => s.SliderGroupId == 4).OrderByDescending(s => s.SliderId).ToList();
            content.ApplyTranslations(db);
            List<IndexSliderItemViewModel> model = new List<IndexSliderItemViewModel>();
            foreach (var item in content)
            {
                model.Add(new IndexSliderItemViewModel()
                {
                    ImageName = item.ImageName,
                    SliderID = item.SliderId,
                    Title = item.Title,
                    Link = item.Link
                });
            }
            return model;
        }

        public List<IndexSliderItemViewModel> GetBlogSlider()
        {
            IBlogGroupsRepository blogGroupsRepository = new BlogGroupsRepository();
            var blogs = db.Blogs.Where(b => b.IsSlider == true).ToList();
            blogs.ApplyTranslations(db);
            List<IndexSliderItemViewModel> model = new List<IndexSliderItemViewModel>();
            foreach (var item in blogs)
            {
                model.Add(new IndexSliderItemViewModel() 
                {
                    ImageName= item.ImageName,
                    SliderID = item.BlogId,
                    Title = item.Title,
                    ShortDescription = blogGroupsRepository.GetBlogGroupNameByBlogID(item.BlogId),
                    Date = DateConvertor.PassDays(item.CreateDate)
                });
            }
            return model;
        }

        public Slider GetByID(int SliderID)
        {
            var item = db.Sliders.Find(SliderID);
            item.ApplyTranslation(db);
            return item;
        }

        public List<IndexSliderItemViewModel> GetContactUsSlider()
        {
            var content = db.Sliders.Where(s => s.SliderGroupId == 6).OrderByDescending(s => s.SliderId).ToList();
            content.ApplyTranslations(db);
            List<IndexSliderItemViewModel> model = new List<IndexSliderItemViewModel>();
            foreach (var item in content)
            {
                model.Add(new IndexSliderItemViewModel()
                {
                    ImageName = item.ImageName,
                    SliderID = item.SliderId,
                    Title = item.Title,
                    Link = item.Link
                });
            }
            return model;
        }

        public IndexSliderViewModel GetIndexHeader()
        {
            IndexSliderViewModel model = new IndexSliderViewModel();
            model.Desktop = new List<IndexSliderItemViewModel>();
            model.Mobile = new List<IndexSliderItemViewModel>();

            var desktops = db.Sliders.Where(s => s.SliderGroupId == 1).OrderByDescending(s => s.SliderId).ToList();
            desktops.ApplyTranslations(db);
            foreach (var item in desktops)
            {
                model.Desktop.Add(new IndexSliderItemViewModel()
                {
                    SliderID = item.SliderId,
                    ImageName = item.ImageName,
                    Title = item.Title,
                    Link = item.Link
                });
            }

            var mobiles = db.Sliders.Where(s => s.SliderGroupId == 2).OrderByDescending(s => s.SliderId).ToList();
            mobiles.ApplyTranslations(db);
            foreach (var item in mobiles)
            {
                model.Mobile.Add(new IndexSliderItemViewModel()
                {
                    SliderID = item.SliderId,
                    ImageName = item.ImageName,
                    Title = item.Title,
                    Link = item.Link
                });
            }
            return model;
        }

        public List<IndexSliderItemViewModel> GetIndexSecondSlider()
        {
            var content = db.Sliders.Where(s => s.SliderGroupId == 3).OrderByDescending(s => s.SliderId).ToList();
            content.ApplyTranslations(db);
            List<IndexSliderItemViewModel> model = new List<IndexSliderItemViewModel>();
            foreach (var item in content)
            {
                model.Add(new IndexSliderItemViewModel() 
                {
                    ImageName = item.ImageName,
                    SliderID = item.SliderId,
                    Title = item.Title,
                    Link = item.Link
                });
            }
            return model;
        }

        public List<IndexSliderItemViewModel> GetQoutesSlider()
        {
            var content = db.Sliders.Where(s => s.SliderGroupId == 5).OrderByDescending(s => s.SliderId).ToList();
            content.ApplyTranslations(db);
            List<IndexSliderItemViewModel> model = new List<IndexSliderItemViewModel>();
            foreach (var item in content)
            {
                model.Add(new IndexSliderItemViewModel()
                {
                    ImageName = item.ImageName,
                    SliderID = item.SliderId,
                    Title = item.Title,
                    ShortDescription = item.ShortDescription
                });
            }
            return model;
        }

        public List<SliderGroup> GetSliderGroups()
        {
            var groups = db.SliderGroups.ToList();
            groups.ApplyTranslations(db);
            return groups;
        }

        public List<Slider> GetSliders(int GroupID)
        {
            var model = db.Sliders.Where(s => s.SliderGroupId == GroupID).OrderByDescending(s => s.SliderId).ToList();
            model.ApplyTranslations(db);
            return model;
        }

        public bool Update(Slider slider, IFormFile ImageName)
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
                db.Sliders.Update(slider);
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
