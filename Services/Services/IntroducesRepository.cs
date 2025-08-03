using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Services
{
    public class IntroducesRepository : IIntroducesRepository
    {
        TwelveDbContext db = new TwelveDbContext();

        public bool Create(IntroduceCrudViewModel model)
        {
            try
            {
                if (!model.SelectedFeature.Any())
                {
                    return false;
                }
                foreach (var featureID in model.SelectedFeature)
                {
                    Introduce introduce = new Introduce()
                    {
                        IntroduceTitle = model.Title,
                        IntroduceText = model.Description,
                        FeatureId = featureID
                    };
                    db.Introduces.Add(introduce);
                    db.SaveChanges();
                    model.IntroduceID = introduce.IntroduceId;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int introduceID)
        {
            try
            {
                var introduce = db.Introduces.Find(introduceID);
                db.Introduces.Remove(introduce);
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

        public IntroduceCrudViewModel GetByID(int introduceID)
        {
            var introduce = db.Introduces.Find(introduceID);
            var translations = db.EntityTranslations.Where(t => t.EntityName == nameof(Introduce) && t.EntityId == introduceID).ToList();
            IntroduceCrudViewModel model = new IntroduceCrudViewModel()
            {
                IntroduceID = introduceID,
                Title = introduce.IntroduceTitle,
                Description = introduce.IntroduceText,
                TitleEn = translations.FirstOrDefault(t => t.Property == nameof(Introduce.IntroduceTitle) && t.Culture == "en")?.Value,
                TitleAr = translations.FirstOrDefault(t => t.Property == nameof(Introduce.IntroduceTitle) && t.Culture == "ar")?.Value,
                TitleUr = translations.FirstOrDefault(t => t.Property == nameof(Introduce.IntroduceTitle) && t.Culture == "ur")?.Value,
                DescriptionEn = translations.FirstOrDefault(t => t.Property == nameof(Introduce.IntroduceText) && t.Culture == "en")?.Value,
                DescriptionAr = translations.FirstOrDefault(t => t.Property == nameof(Introduce.IntroduceText) && t.Culture == "ar")?.Value,
                DescriptionUr = translations.FirstOrDefault(t => t.Property == nameof(Introduce.IntroduceText) && t.Culture == "ur")?.Value
            };
            model.SelectedFeature = new List<int>();
            model.Features = new List<FeatureNameViewModel>();
            model.SelectedFeature.Add(introduce.FeatureId);
            var features = db.Features.ToList();
            features.ApplyTranslations(db);
            foreach (var item in features)
            {
                model.Features.Add(new FeatureNameViewModel()
                {
                    FeatureID = item.FeatureId,
                    Title = item.Title,
                });
            }
            return model;
        }

        public List<IntroducesPageDataViewModel> GetIntroduces()
        {
            var content = db.Introduces.ToList();
            content.ApplyTranslations(db);
            List<IntroducesPageDataViewModel> model = new List<IntroducesPageDataViewModel>();
            foreach (var item in content)
            {
                var feature = db.Features.Find(item.FeatureId);
                feature.ApplyTranslation(db);
                model.Add(new IntroducesPageDataViewModel
                {
                    IntroduceID = item.IntroduceId,
                    Title = item.IntroduceTitle,
                    Description = item.IntroduceText,
                    Feature = feature.Title
                });
            }
            return model;
        }

        public IntroduceCrudViewModel GetModelForCreate()
        {
            IntroduceCrudViewModel model = new IntroduceCrudViewModel();
            model.Features = new List<FeatureNameViewModel>();
            var features = db.Features.ToList();
            features.ApplyTranslations(db);
            foreach (var item in features)
            {
                model.Features.Add(new FeatureNameViewModel() 
                {
                    FeatureID = item.FeatureId,
                    Title = item.Title,
                });
            }
            return model;
        }

        public bool Update(IntroduceCrudViewModel model)
        {
            try
            {
                Introduce introduce = db.Introduces.Find(model.IntroduceID) ;
                introduce.IntroduceTitle = model.Title;
                introduce.IntroduceText = model.Description;
                introduce.FeatureId = model.SelectedFeature.FirstOrDefault();

                db.Introduces.Update(introduce);
                db.SaveChanges();

                return true;
            }
            catch 
            {
                return false;
            }
        }

        public void SaveTranslations(IntroduceCrudViewModel model)
        {
            SaveTranslation(nameof(Introduce), model.IntroduceID, nameof(Introduce.IntroduceTitle), "en", model.TitleEn);
            SaveTranslation(nameof(Introduce), model.IntroduceID, nameof(Introduce.IntroduceTitle), "ar", model.TitleAr);
            SaveTranslation(nameof(Introduce), model.IntroduceID, nameof(Introduce.IntroduceTitle), "ur", model.TitleUr);
            SaveTranslation(nameof(Introduce), model.IntroduceID, nameof(Introduce.IntroduceText), "en", model.DescriptionEn);
            SaveTranslation(nameof(Introduce), model.IntroduceID, nameof(Introduce.IntroduceText), "ar", model.DescriptionAr);
            SaveTranslation(nameof(Introduce), model.IntroduceID, nameof(Introduce.IntroduceText), "ur", model.DescriptionUr);
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
