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
            var item = db.CallInfoes.Find(InfoID);
            item.ApplyTranslation(db);
            return item;
        }

        public CallInfoCrudViewModel GetForEdit(int infoID)
        {
            var call = db.CallInfoes.Find(infoID);
            var model = new CallInfoCrudViewModel
            {
                CallInfoId = call.CallInfoId,
                Title = call.Title,
                ShortDescription = call.ShortDescription,
                Description = call.Description,
                ImageName = call.ImageName
            };
            var tr = db.EntityTranslations.ToList();
            model.TitleEn = tr.FirstOrDefault(t => t.EntityName == nameof(CallInfo) && t.EntityId == infoID && t.Property == nameof(CallInfo.Title) && t.Culture == "en")?.Value;
            model.TitleAr = tr.FirstOrDefault(t => t.EntityName == nameof(CallInfo) && t.EntityId == infoID && t.Property == nameof(CallInfo.Title) && t.Culture == "ar")?.Value;
            model.TitleUr = tr.FirstOrDefault(t => t.EntityName == nameof(CallInfo) && t.EntityId == infoID && t.Property == nameof(CallInfo.Title) && t.Culture == "ur")?.Value;
            model.ShortDescriptionEn = tr.FirstOrDefault(t => t.EntityName == nameof(CallInfo) && t.EntityId == infoID && t.Property == nameof(CallInfo.ShortDescription) && t.Culture == "en")?.Value;
            model.ShortDescriptionAr = tr.FirstOrDefault(t => t.EntityName == nameof(CallInfo) && t.EntityId == infoID && t.Property == nameof(CallInfo.ShortDescription) && t.Culture == "ar")?.Value;
            model.ShortDescriptionUr = tr.FirstOrDefault(t => t.EntityName == nameof(CallInfo) && t.EntityId == infoID && t.Property == nameof(CallInfo.ShortDescription) && t.Culture == "ur")?.Value;
            model.DescriptionEn = tr.FirstOrDefault(t => t.EntityName == nameof(CallInfo) && t.EntityId == infoID && t.Property == nameof(CallInfo.Description) && t.Culture == "en")?.Value;
            model.DescriptionAr = tr.FirstOrDefault(t => t.EntityName == nameof(CallInfo) && t.EntityId == infoID && t.Property == nameof(CallInfo.Description) && t.Culture == "ar")?.Value;
            model.DescriptionUr = tr.FirstOrDefault(t => t.EntityName == nameof(CallInfo) && t.EntityId == infoID && t.Property == nameof(CallInfo.Description) && t.Culture == "ur")?.Value;
            return model;
        }

        public List<DataLayer.CallInfo> GetCallInfoes()
        {
            var items = db.CallInfoes.ToList();
            items.ApplyTranslations(db);
            return items;
        }

        public List<CallInfoLink> GetCallInfoLinks(int CallInfoID)
        {
            var items = db.CallInfoLinks.Where(c => c.CallInfoId == CallInfoID).ToList();
            items.ApplyTranslations(db);
            return items;
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

        public void SaveTranslations(CallInfoCrudViewModel callInfo)
        {
            SaveTranslation(nameof(CallInfo), callInfo.CallInfoId, nameof(CallInfo.Title), "en", callInfo.TitleEn);
            SaveTranslation(nameof(CallInfo), callInfo.CallInfoId, nameof(CallInfo.Title), "ar", callInfo.TitleAr);
            SaveTranslation(nameof(CallInfo), callInfo.CallInfoId, nameof(CallInfo.Title), "ur", callInfo.TitleUr);
            SaveTranslation(nameof(CallInfo), callInfo.CallInfoId, nameof(CallInfo.ShortDescription), "en", callInfo.ShortDescriptionEn);
            SaveTranslation(nameof(CallInfo), callInfo.CallInfoId, nameof(CallInfo.ShortDescription), "ar", callInfo.ShortDescriptionAr);
            SaveTranslation(nameof(CallInfo), callInfo.CallInfoId, nameof(CallInfo.ShortDescription), "ur", callInfo.ShortDescriptionUr);
            SaveTranslation(nameof(CallInfo), callInfo.CallInfoId, nameof(CallInfo.Description), "en", callInfo.DescriptionEn);
            SaveTranslation(nameof(CallInfo), callInfo.CallInfoId, nameof(CallInfo.Description), "ar", callInfo.DescriptionAr);
            SaveTranslation(nameof(CallInfo), callInfo.CallInfoId, nameof(CallInfo.Description), "ur", callInfo.DescriptionUr);
        }

        private void SaveTranslation(string entityName, int entityId, string property, string culture, string value)
        {
            var tr = db.EntityTranslations.FirstOrDefault(t => t.EntityName == entityName && t.EntityId == entityId && t.Property == property && t.Culture == culture);
            if (tr == null)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    db.EntityTranslations.Add(new EntityTranslation
                    {
                        EntityName = entityName,
                        EntityId = entityId,
                        Property = property,
                        Culture = culture,
                        Value = value
                    });
                    db.SaveChanges();
                }
            }
            else
            {
                tr.Value = value;
                db.EntityTranslations.Update(tr);
                db.SaveChanges();
            }
        }
    }
}
