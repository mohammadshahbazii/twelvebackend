using DataLayer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IAboutUsSightRepository : IDisposable
    {
        public List<AboutUsSight> GetAboutUsSights();

        public AboutUsSight GetByID(int aboutUsSightID);
        public bool Create(AboutUsSight sight , IFormFile ImageName);
        public bool Update(AboutUsSight sight , IFormFile ImageName);
        public bool Delete(AboutUsSight sight);
    }
}
