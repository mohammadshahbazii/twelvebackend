using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ViewModels;
using System.Globalization;

namespace Services
{
    public class FaqsRepository : IFaqsRepository
    {
        TwelveDbContext db = new TwelveDbContext();
        private string CurrentCulture => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        public void Dispose()
        {
            db.Dispose();
        }

        public string GetFaqGroupNameByID(int GroupID)
        {
            var item = db.FaqGroups.Find(GroupID);
            item.ApplyTranslation(db);
            return item.GroupName;
        }

        public List<FaqsGroupsItemViewModel> GetFaqGroups()
        {
            var items = db.FaqGroups.ToList();
            items.ApplyTranslations(db);
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
                    var tr = db.FaqTranslations.FirstOrDefault(t => t.FaqId == item.FaqId && t.Language == CurrentCulture);
                    var groups = db.SelectedFaqGroups.Where(g => g.FaqId == item.FaqId).Select(f => f.FaqGroup).ToList();
                    groups.ApplyTranslations(db);
                    model.Faqs.Add(new FaqsItemViewModel()
                    {
                        FaqID = item.FaqId,
                        Answer = tr?.Answer ?? item.Answer,
                        Question = tr?.Question ?? item.Question,
                        GroupName = string.Join(" ، ", groups.Select(g => g.GroupName))
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
                    var tr = db.FaqTranslations.FirstOrDefault(t => t.FaqId == item.FaqId && t.Language == CurrentCulture);
                    var groups = db.SelectedFaqGroups.Where(g => g.FaqId == item.FaqId).Select(f => f.FaqGroup).ToList();
                    groups.ApplyTranslations(db);
                    model.Faqs.Add(new FaqsItemViewModel()
                    {
                        FaqID = item.FaqId,
                        Answer = tr?.Answer ?? item.Answer,
                        Question = tr?.Question ?? item.Question,
                        GroupName = string.Join(" ، ", groups.Select(g => g.GroupName))
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
                var tr = db.FaqTranslations.FirstOrDefault(t => t.FaqId == item.FaqId && t.Language == CurrentCulture);
                model.Add(new FaqsItemViewModel()
                {
                    FaqID = item.FaqId,
                    Answer = tr?.Answer ?? item.Answer,
                    Question = tr?.Question ?? item.Question,
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
                var tr = db.FaqTranslations.FirstOrDefault(t => t.FaqId == faqsItem.FaqId && t.Language == CurrentCulture);
                model.Add(new FaqsItemViewModel()
                {
                    Question = tr?.Question ?? faqsItem.Question,
                    Answer = tr?.Answer ?? faqsItem.Answer,
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
            var trEn = db.FaqTranslations.FirstOrDefault(t => t.FaqId == FaqID && t.Language == "en");
            var trAr = db.FaqTranslations.FirstOrDefault(t => t.FaqId == FaqID && t.Language == "ar");
            var trUr = db.FaqTranslations.FirstOrDefault(t => t.FaqId == FaqID && t.Language == "ur");
            if (trEn != null)
            {
                model.QuestionEn = trEn.Question;
                model.AnswerEn = trEn.Answer;
            }
            if (trAr != null)
            {
                model.QuestionAr = trAr.Question;
                model.AnswerAr = trAr.Answer;
            }
            if (trUr != null)
            {
                model.QuestionUr = trUr.Question;
                model.AnswerUr = trUr.Answer;
            }
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

        public void SaveTranslations(FaqsCrudViewModel faqs)
        {
            SaveFaqTranslation(faqs.FaqID, "en", faqs.QuestionEn, faqs.AnswerEn);
            SaveFaqTranslation(faqs.FaqID, "ar", faqs.QuestionAr, faqs.AnswerAr);
            SaveFaqTranslation(faqs.FaqID, "ur", faqs.QuestionUr, faqs.AnswerUr);
        }

        private void SaveFaqTranslation(int faqId, string lang, string question, string answer)
        {
            if (string.IsNullOrWhiteSpace(question) && string.IsNullOrWhiteSpace(answer))
            {
                return;
            }
            var tr = db.FaqTranslations.FirstOrDefault(t => t.FaqId == faqId && t.Language == lang);
            if (tr == null)
            {
                tr = new FaqTranslation { FaqId = faqId, Language = lang };
                db.FaqTranslations.Add(tr);
            }
            tr.Question = question ?? "";
            tr.Answer = answer ?? "";
            db.SaveChanges();
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
                faqs.FaqID = faq.FaqId;
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

        public FaqGroupCrudViewModel GetGroupForEdit(int id)
        {
            var group = db.FaqGroups.Find(id);
            var translations = db.EntityTranslations.Where(t => t.EntityName == nameof(FaqGroup) && t.EntityId == id).ToList();
            return new FaqGroupCrudViewModel
            {
                FaqGroupId = group.FaqGroupId,
                GroupName = group.GroupName,
                ParentId = group.ParentId,
                GroupNameEn = translations.FirstOrDefault(t => t.Property == nameof(FaqGroup.GroupName) && t.Culture == "en")?.Value,
                GroupNameAr = translations.FirstOrDefault(t => t.Property == nameof(FaqGroup.GroupName) && t.Culture == "ar")?.Value,
                GroupNameUr = translations.FirstOrDefault(t => t.Property == nameof(FaqGroup.GroupName) && t.Culture == "ur")?.Value
            };
        }

        public FaqContent GetFaqContent()
        {
            var item = db.FaqContents.FirstOrDefault();
            item.ApplyTranslation(db);
            return item;
        }

        public FaqContentCrudViewModel GetFaqContentForEdit()
        {
            var item = db.FaqContents.FirstOrDefault();
            var translations = db.EntityTranslations.Where(t => t.EntityName == nameof(FaqContent) && t.EntityId == item.FaqContentId).ToList();
            return new FaqContentCrudViewModel
            {
                FaqContentId = item.FaqContentId,
                FaqContentTitle = item.FaqContentTitle,
                FaqContentSubTitle = item.FaqContentSubTitle,
                FaqContentDescription = item.FaqContentDescription,
                FaqContentTitleEn = translations.FirstOrDefault(t => t.Property == nameof(FaqContent.FaqContentTitle) && t.Culture == "en")?.Value,
                FaqContentTitleAr = translations.FirstOrDefault(t => t.Property == nameof(FaqContent.FaqContentTitle) && t.Culture == "ar")?.Value,
                FaqContentTitleUr = translations.FirstOrDefault(t => t.Property == nameof(FaqContent.FaqContentTitle) && t.Culture == "ur")?.Value,
                FaqContentSubTitleEn = translations.FirstOrDefault(t => t.Property == nameof(FaqContent.FaqContentSubTitle) && t.Culture == "en")?.Value,
                FaqContentSubTitleAr = translations.FirstOrDefault(t => t.Property == nameof(FaqContent.FaqContentSubTitle) && t.Culture == "ar")?.Value,
                FaqContentSubTitleUr = translations.FirstOrDefault(t => t.Property == nameof(FaqContent.FaqContentSubTitle) && t.Culture == "ur")?.Value,
                FaqContentDescriptionEn = translations.FirstOrDefault(t => t.Property == nameof(FaqContent.FaqContentDescription) && t.Culture == "en")?.Value,
                FaqContentDescriptionAr = translations.FirstOrDefault(t => t.Property == nameof(FaqContent.FaqContentDescription) && t.Culture == "ar")?.Value,
                FaqContentDescriptionUr = translations.FirstOrDefault(t => t.Property == nameof(FaqContent.FaqContentDescription) && t.Culture == "ur")?.Value
            };
        }

        public bool UpdateFaqContent(FaqContentCrudViewModel content)
        {
            try
            {
                var entity = db.FaqContents.Find(content.FaqContentId);
                entity.FaqContentTitle = content.FaqContentTitle;
                entity.FaqContentSubTitle = content.FaqContentSubTitle;
                entity.FaqContentDescription = content.FaqContentDescription;
                db.FaqContents.Update(entity);
                db.SaveChanges();
                SaveTranslation(nameof(FaqContent), entity.FaqContentId, nameof(FaqContent.FaqContentTitle), "en", content.FaqContentTitleEn);
                SaveTranslation(nameof(FaqContent), entity.FaqContentId, nameof(FaqContent.FaqContentTitle), "ar", content.FaqContentTitleAr);
                SaveTranslation(nameof(FaqContent), entity.FaqContentId, nameof(FaqContent.FaqContentTitle), "ur", content.FaqContentTitleUr);
                SaveTranslation(nameof(FaqContent), entity.FaqContentId, nameof(FaqContent.FaqContentSubTitle), "en", content.FaqContentSubTitleEn);
                SaveTranslation(nameof(FaqContent), entity.FaqContentId, nameof(FaqContent.FaqContentSubTitle), "ar", content.FaqContentSubTitleAr);
                SaveTranslation(nameof(FaqContent), entity.FaqContentId, nameof(FaqContent.FaqContentSubTitle), "ur", content.FaqContentSubTitleUr);
                SaveTranslation(nameof(FaqContent), entity.FaqContentId, nameof(FaqContent.FaqContentDescription), "en", content.FaqContentDescriptionEn);
                SaveTranslation(nameof(FaqContent), entity.FaqContentId, nameof(FaqContent.FaqContentDescription), "ar", content.FaqContentDescriptionAr);
                SaveTranslation(nameof(FaqContent), entity.FaqContentId, nameof(FaqContent.FaqContentDescription), "ur", content.FaqContentDescriptionUr);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SaveGroupTranslations(FaqGroupCrudViewModel group)
        {
            SaveTranslation(nameof(FaqGroup), group.FaqGroupId, nameof(FaqGroup.GroupName), "en", group.GroupNameEn);
            SaveTranslation(nameof(FaqGroup), group.FaqGroupId, nameof(FaqGroup.GroupName), "ar", group.GroupNameAr);
            SaveTranslation(nameof(FaqGroup), group.FaqGroupId, nameof(FaqGroup.GroupName), "ur", group.GroupNameUr);
        }

        private void SaveTranslation(string entityName, int entityId, string property, string culture, string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            var tr = db.EntityTranslations.FirstOrDefault(t => t.EntityName == entityName && t.EntityId == entityId && t.Property == property && t.Culture == culture);
            if (tr == null)
            {
                tr = new EntityTranslation
                {
                    EntityName = entityName,
                    EntityId = entityId,
                    Property = property,
                    Culture = culture
                };
                db.EntityTranslations.Add(tr);
            }
            tr.Value = value;
            db.SaveChanges();
        }
    }
}
