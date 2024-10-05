using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ViewModels;

namespace Services
{
    public class FaqsRepository : IFaqsRepository
    {
        TwelveDbContext db = new TwelveDbContext();
        public void Dispose()
        {
            db.Dispose();
        }

        public string GetFaqGroupNameByID(int GroupID)
        {
            return db.FaqGroups.Find(GroupID).GroupName;
        }

        public List<FaqsGroupsItemViewModel> GetFaqGroups()
        {
            var items = db.FaqGroups.ToList();
            List<FaqsGroupsItemViewModel> model = new List<FaqsGroupsItemViewModel>();

            foreach (var item in items)
            {
                model.Add(new FaqsGroupsItemViewModel 
                {
                    GroupID = item.FaqGroupId,
                    GroupName = item.GroupName
                });
            }
            return model;
        }
        public PaginationViewModel GetPagination(double PageCount, int PageID = 1)
        {
            PaginationViewModel pagination = new PaginationViewModel();
            pagination.PageNumbers = new List<int>();
            int page = Convert.ToInt32(PageCount) + 1;
            if (PageID < 3)
            {
                for (int i = 1; i < 6; i++)
                {
                    if (i < page)
                    {
                        pagination.PageNumbers.Add(i);
                    }
                }
            }
            else
            {
                for (int i = PageID - 2; i <= PageID + 2; i++)
                {

                    if (i < page)
                    {
                        pagination.PageNumbers.Add(i);
                    }
                }
            }

            pagination.PageCount = Convert.ToInt32(PageCount);
            pagination.PageID = PageID;

            return pagination;
        }


        public AdminFaqsPageDataViewModel GetFaqs(string q = "", int PageID = 1)
        {
            AdminFaqsPageDataViewModel model = new AdminFaqsPageDataViewModel();
            int take = 8;
            int skip = (PageID - 1) * take;
            model.Faqs = new List<FaqsItemViewModel>();
            if (string.IsNullOrEmpty(q))
            {
                var faqs = db.Faqs.Skip(skip).Take(take).ToList();
                foreach (var item in faqs)
                {
                    List<string> groups = db.SelectedFaqGroups.Where(g => g.FaqId == item.FaqId).Select(f => f.FaqGroup.GroupName).ToList();
                    model.Faqs.Add(new FaqsItemViewModel() 
                    {
                        FaqID = item.FaqId,
                        Answer = item.Answer,
                        Question = item.Question,
                        GroupName = string.Join(" ، ", groups)
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.Faqs.ToList().Count()) / Convert.ToDouble(take));
                model.PageCount = pCount;
                model.Pagination = GetPagination(pCount, PageID);
                return model;
            }
            else
            {
                var faqs = db.Faqs.Where(f => f.Question.Contains(q) || f.Answer.Contains(q)).Skip(skip).Take(take).ToList();
                foreach (var item in faqs)
                {
                    List<string> groups = db.SelectedFaqGroups.Where(g => g.FaqId == item.FaqId).Select(f => f.FaqGroup.GroupName).ToList();
                    model.Faqs.Add(new FaqsItemViewModel()
                    {
                        FaqID = item.FaqId,
                        Answer = item.Answer,
                        Question = item.Question,
                        GroupName = string.Join(" ، ", groups)
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.Faqs.Where(f => f.Question.Contains(q) || f.Answer.Contains(q)).ToList().Count()) / Convert.ToDouble(take));
                model.PageCount = pCount;
                model.Pagination = GetPagination(pCount, PageID);
                return model;
            }
        }

        public List<FaqsItemViewModel> GetFaqsByGroups(int GroupID)
        {
            var items = db.SelectedFaqGroups.Where(s => s.FaqGroupId == GroupID).Select(s=> s.Faq ).ToList();
            List<FaqsItemViewModel> model = new List<FaqsItemViewModel>();
            foreach (var item in items)
            {
                model.Add(new FaqsItemViewModel() 
                {
                    FaqID = item.FaqId,
                    Answer = item.Answer,
                    Question = item.Question,
                    GroupID = GroupID,
                    GroupName = GetFaqGroupNameByID(GroupID)
                });
            }
            return model;
        }

        public List<FaqsItemViewModel> GetMainFaqs()
        {
            var faqs = db.Faqs.Where(f => f.IsMain == true).ToList();
            List<FaqsItemViewModel> model = new List<FaqsItemViewModel>();
            foreach (var faqsItem in faqs)
            {
                model.Add(new FaqsItemViewModel()
                {
                    Question = faqsItem.Question,
                    Answer = faqsItem.Answer,
                    FaqID = faqsItem.FaqId
                });
            }
            return model;
        }

        public FaqsCrudViewModel GetByID(int FaqID)
        {
            var faq = db.Faqs.Find(FaqID);
            FaqsCrudViewModel model = new FaqsCrudViewModel()
            {
                FaqID = FaqID,
                Answer = faq.Answer,
                Question = faq.Question,
                IsMain = faq.IsMain
            };
            var groups = db.FaqGroups.ToList();
            model.Groups = new List<FaqsGroupsItemViewModel>();
            foreach (var group in groups)
            {
                model.Groups.Add(new FaqsGroupsItemViewModel() 
                {
                    GroupID = group.FaqGroupId,
                    GroupName = group.GroupName,
                });
            }
            var selectedGroups = db.SelectedFaqGroups.Where(f => f.FaqId == FaqID).Select(f => f.FaqGroupId).ToList();
            model.SelectedGroups = selectedGroups;
            return model;
        }

        public bool Create(FaqsCrudViewModel faqs)
        {
            try
            {
                if (!faqs.SelectedGroups.Any())
                {
                    return false;
                }
                Faq faq = new Faq() 
                {
                    Question = faqs.Question,
                    Answer = faqs.Answer,
                    IsMain= faqs.IsMain
                };
                db.Faqs.Add(faq);
                db.SaveChanges();
                foreach (var item in faqs.SelectedGroups)
                {
                    var selected = new SelectedFaqGroup() { FaqId = faq.FaqId , FaqGroupId = item };
                    db.SelectedFaqGroups.Add(selected);
                    db.SaveChanges();
                }
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public bool Update(FaqsCrudViewModel faqs)
        {
            try
            {
                if (!faqs.SelectedGroups.Any())
                {
                    return false;
                }
                var faq = db.Faqs.Find(faqs.FaqID);
                faq.Question = faqs.Question;
                faq.Answer = faqs.Answer;
                faq.IsMain = faqs.IsMain;
                db.Faqs.Update(faq);
                db.SaveChanges();

                db.SelectedFaqGroups.Where(f => f.FaqId == faqs.FaqID).ToList().ForEach(f => db.SelectedFaqGroups.Remove(f));
                db.SaveChanges();

                foreach (var item in faqs.SelectedGroups)
                {
                    var selected = new SelectedFaqGroup() { FaqId = faqs.FaqID, FaqGroupId = item };
                    db.SelectedFaqGroups.Add(selected);
                    db.SaveChanges();
                }
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public bool Delete(int FaqID)
        {
            try
            {
                

                db.SelectedFaqGroups.Where(f => f.FaqId == FaqID).ToList().ForEach(f => db.SelectedFaqGroups.Remove(f));
                db.SaveChanges();

                var faq = db.Faqs.Find(FaqID);
                db.Faqs.Remove(faq);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public FaqsCrudViewModel GetModelForCreate()
        {
            FaqsCrudViewModel model = new FaqsCrudViewModel();
            var groups = db.FaqGroups.ToList();
            model.Groups = new List<FaqsGroupsItemViewModel>();
            foreach (var group in groups)
            {
                model.Groups.Add(new FaqsGroupsItemViewModel()
                {
                    GroupID = group.FaqGroupId,
                    GroupName = group.GroupName,
                });
            }
            return model;
        }

        public bool CreateGroup(FaqGroup faqGroup)
        {
            try
            {
                db.FaqGroups.Add(faqGroup);
                db.SaveChanges();
                return true;
            }
            catch 
            {
                return true;
            }
        }

        public bool UpdateGroup(FaqGroup faqGroup)
        {
            try
            {
                db.FaqGroups.Update(faqGroup);
                db.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public bool DeleteGroup(FaqGroup faqGroup)
        {
            try
            {
                db.SelectedFaqGroups.Where(f => f.FaqGroupId == faqGroup.FaqGroupId).ToList().ForEach(f => db.SelectedFaqGroups.Remove(f));
                db.SaveChanges();

                db.FaqGroups.Remove(faqGroup);
                db.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public FaqGroup GetGroupByID(int id)
        {
            return db.FaqGroups.Find(id);
        }

        public FaqContent GetFaqContent()
        {
            return db.FaqContents.FirstOrDefault();
        }

        public bool UpdateFaqContent(FaqContent content )
        {
            try
            {
                db.FaqContents.Update(content);
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
