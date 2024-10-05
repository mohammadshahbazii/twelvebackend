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
}
