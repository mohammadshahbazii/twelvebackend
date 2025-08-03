using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{

    public class IndexContentCrudViewModel
    {
        public int IndexContentId { get; set; }
        public string AboutTitle { get; set; }
        public string AboutSubTitle { get; set; }
        public string AboutDescription { get; set; }
        public string FeatureTitle { get; set; }
        public string FeatureSubTitle { get; set; }
        public string FeatureDescription { get; set; }
        public string FaqTitle { get; set; }
        public string FaqSubTitle { get; set; }
        public string FaqDescription { get; set; }
        public string AboutTitleEn { get; set; }
        public string AboutTitleAr { get; set; }
        public string AboutTitleUr { get; set; }
        public string AboutSubTitleEn { get; set; }
        public string AboutSubTitleAr { get; set; }
        public string AboutSubTitleUr { get; set; }
        public string AboutDescriptionEn { get; set; }
        public string AboutDescriptionAr { get; set; }
        public string AboutDescriptionUr { get; set; }
        public string FeatureTitleEn { get; set; }
        public string FeatureTitleAr { get; set; }
        public string FeatureTitleUr { get; set; }
        public string FeatureSubTitleEn { get; set; }
        public string FeatureSubTitleAr { get; set; }
        public string FeatureSubTitleUr { get; set; }
        public string FeatureDescriptionEn { get; set; }
        public string FeatureDescriptionAr { get; set; }
        public string FeatureDescriptionUr { get; set; }
        public string FaqTitleEn { get; set; }
        public string FaqTitleAr { get; set; }
        public string FaqTitleUr { get; set; }
        public string FaqSubTitleEn { get; set; }
        public string FaqSubTitleAr { get; set; }
        public string FaqSubTitleUr { get; set; }
        public string FaqDescriptionEn { get; set; }
        public string FaqDescriptionAr { get; set; }
        public string FaqDescriptionUr { get; set; }
    }

    public class ContactUsContentCrudViewModel
    {
        public int ContactUsContentId { get; set; }
        public string FirstSubTitle { get; set; }
        public string FirstTitle { get; set; }
        public string FristDescription { get; set; }
        public string SecondSubTitle { get; set; }
        public string SecondTitle { get; set; }
        public string SecondDescription { get; set; }
        public string FirstSubTitleEn { get; set; }
        public string FirstSubTitleAr { get; set; }
        public string FirstSubTitleUr { get; set; }
        public string FirstTitleEn { get; set; }
        public string FirstTitleAr { get; set; }
        public string FirstTitleUr { get; set; }
        public string FristDescriptionEn { get; set; }
        public string FristDescriptionAr { get; set; }
        public string FristDescriptionUr { get; set; }
        public string SecondSubTitleEn { get; set; }
        public string SecondSubTitleAr { get; set; }
        public string SecondSubTitleUr { get; set; }
        public string SecondTitleEn { get; set; }
        public string SecondTitleAr { get; set; }
        public string SecondTitleUr { get; set; }
        public string SecondDescriptionEn { get; set; }
        public string SecondDescriptionAr { get; set; }
        public string SecondDescriptionUr { get; set; }
    }

    public class FeaturesContentCrudViewModel
    {
        public int FeatureContentId { get; set; }
        public string FeaturesTitle { get; set; }
        public string FeatruesSubTitle { get; set; }
        public string FeaturesDescription { get; set; }
        public string FeaturesTitleEn { get; set; }
        public string FeaturesTitleAr { get; set; }
        public string FeaturesTitleUr { get; set; }
        public string FeatruesSubTitleEn { get; set; }
        public string FeatruesSubTitleAr { get; set; }
        public string FeatruesSubTitleUr { get; set; }
        public string FeaturesDescriptionEn { get; set; }
        public string FeaturesDescriptionAr { get; set; }
        public string FeaturesDescriptionUr { get; set; }
    }

    public class SeoTagsViewModel
    {
        public int BlogID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
    }

    public class IntroduceAppViewModel
    {
        public List<IntroduceAppItemViewModel> items { get; set; }
        public List<string> Sliders { get; set; }
    }

    public class AboutUsLogoImagesViewModel
    {
        public List<AboutUsLogoItemViewModel> NormalPics { get; set; }
        public List<AboutUsLogoItemViewModel> MainPics { get; set; }
    }

    public class AboutUsLogoItemViewModel
    {
        public string ImageName { get; set; }
    }

    public class IntroduceAppItemViewModel
    {
        public FeatureNameViewModel Features { get; set; }

        public List<ContentItemViewModel> FeatureItems { get; set; }
    }

    public class ContentItemViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
    }

    public class SearchPageDataViewModel
    {
        public List<FaqsItemViewModel> Faqs { get; set; }
        public List<BlogItemViewModel> Blogs { get; set; }
        public List<FeaturesItemViewModel> Features { get; set; }
    }

    public class OrderItemViewModel
    {
        public int OrderID { get; set; }
        public bool IsFinally { get; set; }
        public string Amount { get; set; }
        public string CreateDate { get; set; }
    }

    public class OrdersPageDataViewModel
    {
        public List<OrderItemViewModel> Orders { get; set; }
        public PaginationViewModel Pagination { get; set; }
        public double PageCount { get; set; }
        public string Sum { get; set; }
    }


    public class PaginationViewModel
    {
        public List<int> PageNumbers { get; set; }

        public int PageID { get; set; }


        public double PageCount { get; set; }
    }
    public class SocialMediaItemViewModel
    {
        public int MediaID { get; set; }
        public string ColorName { get; set; }
        public string ClassName { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public string Link { get; set; }
    }
    public class DownloadLinkViewModel
    {
        public int DownloadID { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string ImageName { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string Link { get; set; }
    }

    public class DownloadLinkGroupViewModel
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
    }

    public class DownloadLinkCrudViewModel
    {
        public int DownloadID { get; set; }
        [Display(Name ="عنوان")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید")]
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string ImageName { get; set; }
        [Display(Name = "لینک")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Link { get; set; }
        public List<DownloadLinkGroupViewModel> Groups { get; set; }
        public List<int> SelectedGroups { get; set; }
    }

    public class IndexDownloadBoxViewModel 
    {
        public string DirectLink { get; set; }
        public List<DownloadLinkViewModel> DownloadLinks { get; set; }
        public List<DownloadLinkGroupViewModel> DownloadLinkGroups { get; set; }
        public List<SocialMediaItemViewModel> SocialMedia { get; set; }
    }
}
