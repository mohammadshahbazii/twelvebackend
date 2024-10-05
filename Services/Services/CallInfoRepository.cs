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
    public class CallInfoRepository : ICallInfoRepository
    {
        TwelveDbContext db = new TwelveDbContext();

        public bool Create(DataLayer.CallInfo callInfo , IFormFile ImageName)
        {
            try
            {
                if (ImageName == null)
                {
                    return false;
                }
                callInfo.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", callInfo.ImageName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageName.CopyTo(stream);
                }
                db.CallInfoes.Add(callInfo);
                db.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public bool Delete(DataLayer.CallInfo callInfo)
        {
            try
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", callInfo.ImageName);
                System.IO.File.Delete(imagePath);
                db.CallInfoes.Remove(callInfo);
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

        public DataLayer.CallInfo GetByID(int InfoID)
        {
            return db.CallInfoes.Find(InfoID);
        }

        public List<DataLayer.CallInfo> GetCallInfoes()
        {
            return db.CallInfoes.ToList();
        }

        public List<CallInfoLink> GetCallInfoLinks(int CallInfoID)
        {
            return db.CallInfoLinks.Where(c => c.CallInfoId == CallInfoID).ToList();
        }

        public bool Update(DataLayer.CallInfo callInfo, IFormFile ImageName)
        {
            try
            {
                if (ImageName != null)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", callInfo.ImageName);

                    System.IO.File.Delete(imagePath);

                    callInfo.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", callInfo.ImageName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        ImageName.CopyTo(stream);
                    }

                }
                db.CallInfoes.Update(callInfo);
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
