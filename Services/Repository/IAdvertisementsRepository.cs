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
    public interface IAdvertisementsRepository : IDisposable
    {
        public List<Advertisement> GetLittleAds();
        public List<Advertisement> GetBanners();
        public bool Create(Advertisement advertisement , IFormFile ImageName);
        public bool Update(Advertisement advertisement , IFormFile ImageName);
        public bool Delete(Advertisement advertisement);
        public Advertisement GetByID(int adsID);
        public List<AdvertisementsItemViewModel> GetAdvertisements();
    }
}
