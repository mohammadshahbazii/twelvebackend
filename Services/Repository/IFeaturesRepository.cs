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
    public interface IFeaturesRepository : IDisposable
    {
        public FeaturesContent GetFeaturesContent();
        public bool UpdateFeaturesContent(FeaturesContent content);

        public IntroduceAppViewModel GetIntroduceApp();
        public List<BlogItemViewModel> GetRelatedNews(int FeatureID);

        public Feature GetByID(int id);
        public FeatureCrudViewModel GetForEdit(int id);
        public bool Create(Feature feature , IFormFile ImageName , IFormFile AnimateFile , IFormFile FirstArticleImage , IFormFile SecondArticleImage, IFormFile IntroduceImage);
        public bool Update(Feature feature , IFormFile ImageName , IFormFile AnimateFile , IFormFile FirstArticleImage , IFormFile SecondArticleImage , IFormFile IntroduceImage);
        public bool Update(Feature feature , IFormFile IntroduceImage);
        public bool Delete(Feature feature);
        public void SaveTranslations(FeatureCrudViewModel feature);
        public AdminFeaturesPageDataViewModel GetAdminFeatures(string q = "" , int PageID =1);
        public PaginationViewModel GetPagination(double PageCount, int PageID = 1);

        public FeaturePageDataViewModel GetFeature(string Demo);
        public List<FeaturesItemViewModel> GetFeatures();

        public List<Feature> GetFeatureIntroduces();

        public FeatureItem GetItemByID(int ItemID);
        public FeatureItemCrudViewModel GetItemForEdit(int ItemID);
        public bool CreateItem(FeatureItem featureItem , IFormFile formFile);
        public bool UpdateItem(FeatureItem featureItem , IFormFile formFile);
        public bool DeleteItem(FeatureItem featureItem);
        public void SaveItemTranslations(FeatureItemCrudViewModel item);

        public List<FeatureIconsViewModel> GetFeaturesIcons(int featureID);
    }
}
