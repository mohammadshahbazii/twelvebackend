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
    public interface IAboutUsSightRepository : IDisposable
    {
        public List<AboutUsSight> GetAboutUsSights();

        public AboutUsSight GetByID(int aboutUsSightID);
        public AboutUsSightCrudViewModel GetForEdit(int aboutUsSightID);
        public bool Create(AboutUsSightCrudViewModel sight, IFormFile ImageName);
        public bool Update(AboutUsSightCrudViewModel sight, IFormFile ImageName);
        public bool Delete(AboutUsSight sight);
        public void SaveTranslations(AboutUsSightCrudViewModel sight);
    }
}
