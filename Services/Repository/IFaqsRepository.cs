using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Services
{
    public interface IFaqsRepository : IDisposable
    {
        public FaqContent GetFaqContent();
        public FaqContentCrudViewModel GetFaqContentForEdit();
        public bool UpdateFaqContent(FaqContentCrudViewModel content);
        public bool CreateGroup(FaqGroup faqGroup);
        public bool UpdateGroup(FaqGroup faqGroup);
        public bool DeleteGroup(FaqGroup faqGroup);

        public FaqGroup GetGroupByID(int id);
        public FaqGroupCrudViewModel GetGroupForEdit(int id);
        public void SaveGroupTranslations(FaqGroupCrudViewModel group);

        public FaqsCrudViewModel GetModelForCreate();
        public FaqsCrudViewModel GetByID(int FaqID);
        public bool Create(FaqsCrudViewModel faqs);
        public bool Update(FaqsCrudViewModel faqs);
        public bool Delete(int FaqID);
        public void SaveTranslations(FaqsCrudViewModel faqs);

        public PaginationViewModel GetPagination(double PageCount, int PageID = 1);

        public AdminFaqsPageDataViewModel GetFaqs(string q = "" , int PageID = 1);
        public string GetFaqGroupNameByID(int GroupID);
        public List<FaqsItemViewModel> GetMainFaqs();

        public List<FaqsGroupsItemViewModel> GetFaqGroups();

        public List<FaqsItemViewModel> GetFaqsByGroups(int GroupID);
    }
}
