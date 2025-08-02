using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class FeatureNameViewModel
    {
        public int FeatureID { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
    }

    public class AdminFeaturesPageDataViewModel
    {
        public List<FeaturesItemViewModel> Features { get; set; }
        public PaginationViewModel Pagination { get; set; }
        public double PageCount { get; set; }
    }
    public class FeaturePageDataViewModel
    {
        public int FeatureID { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string FirstNewsTitle { get; set; }
        public string FirstNewsShortDescription { get; set; }
        public string FirstNewsImageName { get; set; }
        public string SecondNewsTitle { get; set; }
        public string SecondNewsShortDescription { get; set; }
        public string SecondNewsImageName { get; set; }

        public List<FeatureIconsViewModel> Icons { get; set; }

    }

    public class FeatureIconsViewModel
    {
        public int ItemID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
    }

    public class FeaturesItemViewModel
    {
        public int FeatureID { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string ImageName { get; set; }
        public string AnimateImage { get; set; }
        public string Demo { get; set; }
        public bool IsBig { get; set; }
    }

    public class FeatureCrudViewModel
    {
        public int FeatureId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string FirstDescription { get; set; }
        public string FirstArticleTitle { get; set; }
        public string FirstArticleDescription { get; set; }
        public string SecondArticleTitle { get; set; }
        public string SecondArticleDescription { get; set; }
        public string TitleEn { get; set; }
        public string TitleAr { get; set; }
        public string TitleUr { get; set; }
        public string ShortDescriptionEn { get; set; }
        public string ShortDescriptionAr { get; set; }
        public string ShortDescriptionUr { get; set; }
        public string FirstDescriptionEn { get; set; }
        public string FirstDescriptionAr { get; set; }
        public string FirstDescriptionUr { get; set; }
        public string FirstArticleTitleEn { get; set; }
        public string FirstArticleTitleAr { get; set; }
        public string FirstArticleTitleUr { get; set; }
        public string FirstArticleDescriptionEn { get; set; }
        public string FirstArticleDescriptionAr { get; set; }
        public string FirstArticleDescriptionUr { get; set; }
        public string SecondArticleTitleEn { get; set; }
        public string SecondArticleTitleAr { get; set; }
        public string SecondArticleTitleUr { get; set; }
        public string SecondArticleDescriptionEn { get; set; }
        public string SecondArticleDescriptionAr { get; set; }
        public string SecondArticleDescriptionUr { get; set; }
        public string ImageName { get; set; }
        public string AnimateFilename { get; set; }
        public string FirstArticleImage { get; set; }
        public string SecondArticleImage { get; set; }
        public string IntroduceImageName { get; set; }
    }

    public class FeatureItemCrudViewModel
    {
        public int FeaturesItemId { get; set; }
        public int FeatureId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string TitleEn { get; set; }
        public string TitleAr { get; set; }
        public string TitleUr { get; set; }
        public string ShortDescriptionEn { get; set; }
        public string ShortDescriptionAr { get; set; }
        public string ShortDescriptionUr { get; set; }
        public string ImageName { get; set; }
    }
}
