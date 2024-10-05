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
            IntroduceCrudViewModel model = new IntroduceCrudViewModel() 
            {
                Title = introduce.IntroduceTitle,
                Description = introduce.IntroduceText,
            };
            model.SelectedFeature = new List<int>();
            model.Features = new List<FeatureNameViewModel>();
            model.SelectedFeature.Add(introduce.FeatureId);
            var features = db.Features.ToList();
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
            List<IntroducesPageDataViewModel> model = new List<IntroducesPageDataViewModel>();
            foreach (var item in content)
            {
                model.Add(new IntroducesPageDataViewModel 
                {
                    IntroduceID = item.IntroduceId,
                    Title = item.IntroduceTitle,
                    Description = item.IntroduceText,
                    Feature = db.Features.Find(item.FeatureId).Title
                });
            }
            return model;
        }

        public IntroduceCrudViewModel GetModelForCreate()
        {
            IntroduceCrudViewModel model = new IntroduceCrudViewModel();
            model.Features = new List<FeatureNameViewModel>();
            var features = db.Features.ToList();
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
    }
}
