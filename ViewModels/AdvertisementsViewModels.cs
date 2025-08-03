using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class AdvertisementsItemViewModel
    {
        public int AdvertisementID { get; set; }
        public string ImageName { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
    }

    public class AdvertisementCrudViewModel
    {
        public int AdvertisementId { get; set; }
        public string Title { get; set; }
        public string TitleEn { get; set; }
        public string TitleAr { get; set; }
        public string TitleUr { get; set; }
        public string Link { get; set; }
        public bool IsBanner { get; set; }
        public string ImageName { get; set; }
    }
}
