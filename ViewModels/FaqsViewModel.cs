using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class FaqsCrudViewModel
    {
        public int FaqID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool IsMain { get; set; }
        public List<FaqsGroupsItemViewModel> Groups { get; set; }
        public List<int> SelectedGroups { get; set; }
    }

    public class FaqsPageDataViewModel
    {
        public List<FaqsGroupsItemViewModel> Groups { get; set; }
        public List<FaqsItemViewModel> Faqs { get; set; }
    }

    public class AdminFaqsPageDataViewModel
    {
        public List<FaqsItemViewModel> Faqs { get; set; }
        public PaginationViewModel Pagination { get; set; }
        public double PageCount { get; set; }
    }


    public class FaqsGroupsItemViewModel
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
    }

    public class FaqsItemViewModel
    {
        public int FaqID { get; set; }
        public string Question { get; set;}
        public string Answer { get; set;}
        public int GroupID { get; set;}
        public string GroupName { get; set;}
    }
}
