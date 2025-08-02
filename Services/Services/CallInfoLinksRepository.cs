using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CallInfoLinksRepository : ICallInfoLinksRepository
    {
        TwelveDbContext db = new TwelveDbContext();

        public bool Create(CallInfoLink callInfoLink)
        {
            try
            {
                db.CallInfoLinks.Add(callInfoLink);
                db.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public bool Delete(CallInfoLink callInfoLink)
        {
            try
            {
                db.CallInfoLinks.Remove(callInfoLink);
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

        public CallInfoLink GetByID(int LinkID)
        {
            var item = db.CallInfoLinks.Find(LinkID);
            item.ApplyTranslation(db);
            return item;
        }

        public List<CallInfoLink> GetLinks(int CallInfoID)
        {
            var items = db.CallInfoLinks.Where(l => l.CallInfoId == CallInfoID).ToList();
            items.ApplyTranslations(db);
            return items;
        }

        public bool Update(CallInfoLink callInfoLink)
        {
            try
            {
                db.CallInfoLinks.Update(callInfoLink);
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
