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
    public class FeedBacksRepository : IFeedBacksRepository
    {
        TwelveDbContext db =new TwelveDbContext();
        public void Dispose()
        {
            db.Dispose();
        }

        public PaginationViewModel GetPagination(double PageCount, int PageID = 1)
        {
            PaginationViewModel pagination = new PaginationViewModel();
            pagination.PageNumbers = new List<int>();
            int page = Convert.ToInt32(PageCount) + 1;
            if (PageID < 3)
            {
                for (int i = 1; i < 6; i++)
                {
                    if (i < page)
                    {
                        pagination.PageNumbers.Add(i);
                    }
                }
            }
            else
            {
                for (int i = PageID - 2; i <= PageID + 2; i++)
                {

                    if (i < page)
                    {
                        pagination.PageNumbers.Add(i);
                    }
                }
            }

            pagination.PageCount = Convert.ToInt32(PageCount);
            pagination.PageID = PageID;

            return pagination;
        }

        public FeedBacksPageDataViewModel GetFeedBacks(string q = "", int PageID = 1)
        {
            FeedBacksPageDataViewModel model = new FeedBacksPageDataViewModel();
            int take = 8;
            int skip = (PageID - 1) * take;
            model.FeedBacks = new List<FeedBacksItemViewModels>();
            if (string.IsNullOrEmpty(q))
            {
                var feedbacks = db.FeedBacks.OrderByDescending(f => f.CreateDate).Skip(skip).Take(take).ToList();
                foreach (var item in feedbacks)
                {
                    model.FeedBacks.Add(new FeedBacksItemViewModels()
                    {
                        FeedBackID = item.FeedBackId,
                        Fullname = item.Fullname,
                        Subject = item.Subject,
                        CreateDate = DateConvertor.ToShamsi(item.CreateDate)
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.FeedBacks.ToList().Count()) / Convert.ToDouble(take));
                model.PageCount = pCount;
                model.Pagination = GetPagination(pCount, PageID);
                return model;
            }
            else
            {
                var feedbacks = db.FeedBacks.OrderByDescending(f => f.CreateDate).Where(f => f.Subject.Contains(q)).Skip(skip).Take(take).ToList();
                foreach (var item in feedbacks)
                {
                    model.FeedBacks.Add(new FeedBacksItemViewModels()
                    {
                        FeedBackID = item.FeedBackId,
                        Fullname = item.Fullname,
                        Subject = item.Subject,
                        CreateDate = DateConvertor.ToShamsi(item.CreateDate)
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.FeedBacks.Where(f => f.Subject.Contains(q)).ToList().Count()) / Convert.ToDouble(take));
                model.PageCount = pCount;
                model.Pagination = GetPagination(pCount, PageID);
                return model;
            }
        }

        public List<FeedBacksItemViewModels> GetSelectedFeedBacks()
        {
            var content = db.FeedBacks.Where(f => f.IsShow == true).Take(3).ToList();
            List<FeedBacksItemViewModels> model = new List<FeedBacksItemViewModels>();
            foreach (var item in content)
            {
                model.Add(new FeedBacksItemViewModels() 
                {
                    FeedBackID = item.FeedBackId,
                    Subject = item.Subject,
                    CreateDate= DateConvertor.ToShamsi(item.CreateDate),
                    Fullname = item.Fullname,
                    Text = item.Text,
                    ImageName = item.ImageName
                });
            }
            return model;
        }

        public bool Update(FeedBack feedBack, IFormFile ImageName)
        {
            try
            {
                if (string.IsNullOrEmpty(feedBack.ImageName) && ImageName == null && feedBack.IsShow == true)
                {
                    return false;
                }
                if (ImageName != null)
                {
                    if (!string.IsNullOrEmpty(feedBack.ImageName))
                    {
                        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/FeedBacks", feedBack.ImageName);

                        System.IO.File.Delete(imagePath);
                    }

                    feedBack.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/FeedBacks", feedBack.ImageName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        ImageName.CopyTo(stream);
                    }
                }

                
                db.FeedBacks.Update(feedBack);
                db.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public bool Update(FeedBack feedBack)
        {
            try
            {
                db.FeedBacks.Update(feedBack);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public FeedBack GetByID(int id)
        {
            return db.FeedBacks.Find(id);
        }

        public bool Delete(FeedBack feedBack)
        {
            try
            {
                db.FeedBacks.Remove(feedBack);
                db.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public List<FeedBacksItemViewModels> GetAdminSelectedFeedBacks()
        {
            var content = db.FeedBacks.Where(f => f.IsShow == true).ToList();
            List<FeedBacksItemViewModels> model = new List<FeedBacksItemViewModels>();
            foreach (var item in content)
            {
                model.Add(new FeedBacksItemViewModels()
                {
                    FeedBackID = item.FeedBackId,
                    Subject = item.Subject,
                    CreateDate = DateConvertor.ToShamsi(item.CreateDate),
                    Fullname = item.Fullname,
                    Text = item.Text,
                    ImageName = item.ImageName
                });
            }
            return model;
        }
    }
}
