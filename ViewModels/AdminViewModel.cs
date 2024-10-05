using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class AdminCrudViewModel
    {
        public int AdminID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phonenumber { get; set; }

        public List<AdminAccessViewModel> Access { get; set; }
        public List<int> SelectedAccess { get; set; }
    }

    public class AdminAccessViewModel
    {
        public int AccessID { get; set; }
        public string AccessName { get; set; }
    }

    public class AdminsPageDataViewModel
    {
        public List<AdminItemViewModel> Admin { get; set; }
        public PaginationViewModel Pagination { get; set; }
        public double PageCount { get; set; }
    }

    public class AdminItemViewModel
    {
        public int AdminID { get; set; }
        public string Username { get; set; }
        public string Phonenumber { get; set; }
        public string Access { get; set; }
    }

    public class AdminStatisticsViewModel
    {
        public int SiteVisit { get; set; }
        public int Orders { get; set; }
        public int ConfirmComment { get; set; }
        public int AnswerComment { get; set; }
        public int NewsConfirmComment { get; set; }
        public int NewsAnswerComment { get; set; }
        public int DownloadCount { get; set; }
    }

    public class LoginViewModel
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
