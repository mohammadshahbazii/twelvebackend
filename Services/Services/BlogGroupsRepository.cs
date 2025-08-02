using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Services
{
    public class BlogGroupsRepository : IBlogGroupsRepository
    {
        TwelveDbContext db = new TwelveDbContext();

        public bool Create(BlogGroup blogGroup)
        {
            try
            {
                db.BlogGroups.Add(blogGroup);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(BlogGroup blogGroup)
        {
            try
            {
                db.SelectedBlogGroups.Where(g => g.BlogGroupId == blogGroup.BlogGroupId).ToList().ForEach(g => db.SelectedBlogGroups.Remove(g));
                db.SaveChanges() ;
                db.BlogGroups.Remove(blogGroup);
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

        public List<BlogGroupNameViewModel> GetAllGroups()
        {
            var groups = new List<BlogGroupNameViewModel>();
            var blogGroups = db.BlogGroups.ToList();
            blogGroups.ApplyTranslations(db);
            foreach (var blogGroup in blogGroups)
            {
                groups.Add(new BlogGroupNameViewModel()
                {
                    GroupID = blogGroup.BlogGroupId,
                    GroupName = blogGroup.GroupName
                });
            }
            return groups;
        }


        public string GetBlogGroupNameByBlogID(int BlogID)
        {
            var groupID = db.SelectedBlogGroups.FirstOrDefault(s => s.BlogId == BlogID).BlogGroupId;
            var group = db.BlogGroups.Find(groupID);
            group.ApplyTranslation(db);
            return group.GroupName;
        }

        public BlogGroup GetByID(int id)
        {
            var item = db.BlogGroups.Find(id);
            item.ApplyTranslation(db);
            return item;
        }

        public bool Update(BlogGroup blogGroup)
        {
            try
            {
                db.BlogGroups.Update(blogGroup);
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
