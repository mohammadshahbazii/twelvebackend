using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class IntroducesPageDataViewModel
    {
        public int IntroduceID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Feature { get; set; }
    }

    public class IntroduceCrudViewModel
    {
        public List<FeatureNameViewModel> Features { get; set; }
        public List<int> SelectedFeature { get; set; }
        public int IntroduceID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Feature { get; set; }
    }
}
