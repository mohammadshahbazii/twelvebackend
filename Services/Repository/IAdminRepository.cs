using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Services
{
    public interface IAdminRepository : IDisposable
    {
        public AdminCrudViewModel GetModelForCreate();
        public bool Create(AdminCrudViewModel admin);
        public bool Update(AdminCrudViewModel admin);
        public bool Delete(AdminCrudViewModel admin);
        public AdminCrudViewModel GetByID(int id);
        public AdminsPageDataViewModel GetAdmins(string q="" , int PageID = 1);

        public PaginationViewModel GetPagination(double PageCount, int PageID = 1);


        public bool CheckPermission(string username, string access);
        public AdminStatisticsViewModel GetStatistics();

        public IDictionary<string,int> GetSiteVisitData();
        public Admin GetAdminByUsername(string username);

        public string SendPassword(string username);
        public string SendEmail(string username);

        public bool Verification(LoginViewModel login);
    }
}
