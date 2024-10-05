using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Services
{
    public interface IBlogGroupsRepository : IDisposable
    {
        public bool Create(BlogGroup blogGroup);
        public bool Update(BlogGroup blogGroup);
        public bool Delete(BlogGroup blogGroup);
        public BlogGroup GetByID(int id);
        public List<BlogGroupNameViewModel> GetAllGroups();

        public string GetBlogGroupNameByBlogID(int BlogID);
    }
}
