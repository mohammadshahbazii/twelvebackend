using DataLayer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IIntroduceSlidersRepository : IDisposable
    {
        public List<IntroduceSlider> GetListByFeatureID(int featureID);
        public IntroduceSlider GetByID(int id);
        public bool Create(IntroduceSlider slider , IFormFile ImageName);
        public bool Update(IntroduceSlider slider , IFormFile ImageName);
        public bool Delete(IntroduceSlider slider);
    }
}
