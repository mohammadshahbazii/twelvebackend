using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class FeedBacksPageDataViewModel
    {
        public List<FeedBacksItemViewModels> FeedBacks { get; set; }
        public PaginationViewModel Pagination { get; set; }
        public double PageCount { get; set; }
    }

    public class FeedBacksItemViewModels
    {
        public int FeedBackID { get; set; }
        public string Fullname { get; set; }
        public string Text { get; set; }
        public string Subject { get; set; }
        public string CreateDate { get; set; }
        public string ImageName { get; set; }
    }
}
