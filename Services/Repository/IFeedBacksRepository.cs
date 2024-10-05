using DataLayer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Services
{
    public interface IFeedBacksRepository : IDisposable
    {
        public bool Update(FeedBack feedBack , IFormFile formFile);
        public bool Update(FeedBack feedBack);
        public bool Delete(FeedBack feedBack);
        public FeedBack GetByID(int id);
        public PaginationViewModel GetPagination(double PageCount, int PageID = 1);

        public FeedBacksPageDataViewModel GetFeedBacks(string q = "", int PageID = 1);
        public List<FeedBacksItemViewModels> GetSelectedFeedBacks();
        public List<FeedBacksItemViewModels> GetAdminSelectedFeedBacks();
    }
}
