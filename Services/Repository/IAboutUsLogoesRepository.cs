using DataLayer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository
{
    public interface IAboutUsLogoesRepository : IDisposable
    {
        public List<AboutUsLogo> GetAboutUsLogos();
        public AboutUsLogo GetByID(int id);
        public bool Create(AboutUsLogo logo , IFormFile ImageName);
        public bool Update(AboutUsLogo logo , IFormFile ImageName);
        public bool Delete(AboutUsLogo logo);
    }
}
