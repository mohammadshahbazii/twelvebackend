using DataLayer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IAboutUsItemsRepository: IDisposable
    {
        public List<AboutUsItem> GetAboutUsItems();
        public AboutUsItem GetByID(int itemID);
        public bool Create(AboutUsItem item , IFormFile ImageName);
        public bool Update(AboutUsItem item , IFormFile ImageName);
        public bool Delete(AboutUsItem item);
    }
}
