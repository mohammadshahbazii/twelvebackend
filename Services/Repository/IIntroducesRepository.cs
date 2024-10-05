using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Services
{
    public interface IIntroducesRepository : IDisposable
    {
        public List<IntroducesPageDataViewModel> GetIntroduces();
        public bool Create(IntroduceCrudViewModel model);
        public bool Update(IntroduceCrudViewModel model);
        public bool Delete(int introduceID);

        public IntroduceCrudViewModel GetModelForCreate();
        public IntroduceCrudViewModel GetByID(int introduceID);
    }
}
