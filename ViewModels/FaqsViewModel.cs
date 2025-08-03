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
        public string QuestionEn { get; set; }
        public string QuestionAr { get; set; }
        public string QuestionUr { get; set; }
        public string AnswerEn { get; set; }
        public string AnswerAr { get; set; }
        public string AnswerUr { get; set; }
        public bool IsMain { get; set; }
        public List<FaqsGroupsItemViewModel> Groups { get; set; }
        public List<int> SelectedGroups { get; set; }
    }

    public class FaqGroupCrudViewModel
    {
        public int FaqGroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupNameEn { get; set; }
        public string GroupNameAr { get; set; }
        public string GroupNameUr { get; set; }
        public int? ParentId { get; set; }
    }

    public class FaqContentCrudViewModel
    {
        public int FaqContentId { get; set; }
        public string FaqContentTitle { get; set; }
        public string FaqContentSubTitle { get; set; }
        public string FaqContentDescription { get; set; }
        public string FaqContentTitleEn { get; set; }
        public string FaqContentTitleAr { get; set; }
        public string FaqContentTitleUr { get; set; }
        public string FaqContentSubTitleEn { get; set; }
        public string FaqContentSubTitleAr { get; set; }
        public string FaqContentSubTitleUr { get; set; }
        public string FaqContentDescriptionEn { get; set; }
        public string FaqContentDescriptionAr { get; set; }
        public string FaqContentDescriptionUr { get; set; }
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
