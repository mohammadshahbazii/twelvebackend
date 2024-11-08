using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{

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
