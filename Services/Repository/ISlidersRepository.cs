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
    public interface ISlidersRepository : IDisposable
    {
        public bool Delete(Slider slider);
        public bool Update(Slider slider , IFormFile ImageName);
        public bool Create(Slider slider, IFormFile ImageName);
        public Slider GetByID(int SliderID);
        public SliderCrudViewModel GetForEdit(int sliderId);
        public List<Slider> GetSliders(int GroupID);
        public List<SliderGroup> GetSliderGroups();
        public IndexSliderViewModel GetIndexHeader();
        public List<IndexSliderItemViewModel> GetIndexSecondSlider();
        public List<IndexSliderItemViewModel> GetAboutUsSlider();
        public List<IndexSliderItemViewModel> GetContactUsSlider();
        public List<IndexSliderItemViewModel> GetBlogSlider();
        public List<IndexSliderItemViewModel> GetQoutesSlider();
        public void SaveTranslations(SliderCrudViewModel slider);
    }
}
