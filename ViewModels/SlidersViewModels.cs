using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class IndexSliderItemViewModel
    {
        public int SliderID { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string ImageName { get; set; }
        public string Date { get; set; }
        public string Link { get; set; }
    }

    public class IndexSliderViewModel
    {
        public List<IndexSliderItemViewModel> Desktop { get; set; }
        public List<IndexSliderItemViewModel> Mobile { get; set; }
    }
}
