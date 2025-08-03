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
    public interface IAboutUsItemsRepository: IDisposable
    {
        public List<AboutUsItem> GetAboutUsItems();
        public AboutUsItem GetByID(int itemID);
        public AboutUsItemCrudViewModel GetForEdit(int itemID);
        public bool Create(AboutUsItemCrudViewModel item, IFormFile ImageName);
        public bool Update(AboutUsItemCrudViewModel item, IFormFile ImageName);
        public bool Delete(AboutUsItem item);
        public void SaveTranslations(AboutUsItemCrudViewModel item);
    }
}
