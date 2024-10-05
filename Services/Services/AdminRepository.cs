using DataLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using ViewModels;

namespace Services
{
    public class AdminRepository : IAdminRepository
    {
        TwelveDbContext db = new TwelveDbContext();

        public bool CheckPermission(string username, string access)
        {
            int adminID = GetAdminByUsername(username).AdminId ;
            int accessID = db.Acesses.FirstOrDefault(a => a.AccessText == access).AccessId;

            if (db.SelectedAdminAccesses.Any(a => a.AdminId == adminID && a.AccessId == accessID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public PaginationViewModel GetPagination(double PageCount, int PageID = 1)
        {
            PaginationViewModel pagination = new PaginationViewModel();
            pagination.PageNumbers = new List<int>();
            int page = Convert.ToInt32(PageCount) + 1;
            if (PageID < 3)
            {
                for (int i = 1; i < 6; i++)
                {
                    if (i < page)
                    {
                        pagination.PageNumbers.Add(i);
                    }
                }
            }
            else
            {
                for (int i = PageID - 2; i <= PageID + 2; i++)
                {

                    if (i < page)
                    {
                        pagination.PageNumbers.Add(i);
                    }
                }
            }

            pagination.PageCount = Convert.ToInt32(PageCount);
            pagination.PageID = PageID;

            return pagination;
        }

        public Admin GetAdminByUsername(string username)
        {
            return db.Admins.FirstOrDefault(a => a.UserName == username);
        }

        public AdminsPageDataViewModel GetAdmins(string q = "", int PageID = 1)
        {
            AdminsPageDataViewModel model = new AdminsPageDataViewModel();
            int take = 8;
            int skip = (PageID - 1) * take;
            model.Admin = new List<AdminItemViewModel>();
            if (string.IsNullOrEmpty(q))
            {
                var admins = db.Admins.Skip(skip).Take(take).ToList();
                foreach (var item in admins)
                {
                    List<string> access = db.SelectedAdminAccesses.Where(a => a.AdminId == item.AdminId).Include(a => a.Access).Select(a => a.Access.AccessText).ToList();

                    model.Admin.Add(new AdminItemViewModel()
                    {
                        AdminID = item.AdminId,
                        Username = item.UserName,
                        Phonenumber = item.PhoneNumber,
                        Access = string.Join(" ، ", access)
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.Admins.ToList().Count()) / Convert.ToDouble(take));
                model.PageCount = pCount;
                model.Pagination = GetPagination(pCount, PageID);
                return model;
            }
            else
            {
                var admins = db.Admins.Where(a => a.UserName.Contains(q)).Skip(skip).Take(take).ToList();
                foreach (var item in admins)
                {
                    List<string> access = db.SelectedAdminAccesses.Where(a => a.AdminId == item.AdminId).Include(a => a.Access).Select(a => a.Access.AccessText).ToList();

                    model.Admin.Add(new AdminItemViewModel()
                    {
                        AdminID = item.AdminId,
                        Phonenumber = item.PhoneNumber,
                        Username = item.UserName,
                        Access = string.Join(" ، ", access)
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.Admins.Where(a => a.UserName.Contains(q)).ToList().Count()) / Convert.ToDouble(take));
                model.PageCount = pCount;
                model.Pagination = GetPagination(pCount, PageID);
                return model;
            }
        }

        public IDictionary<string, int> GetSiteVisitData()
        {
            PersianCalendar pc = new PersianCalendar();
            IDictionary<string, int> model = new Dictionary<string, int>();
            var year = DateTime.Now.Year;
            var content = db.SiteVisits.Where(s => s.CreateDate.Year >= year).ToList();
            for (int i = 1;i <=12;i++)
            {
                var count = content.Where(s => s.CreateDate.Month == i).Count();
                model.Add(DateConvertor.GetPersianMonth(i),count);
            }
            return model;
        }

        public AdminStatisticsViewModel GetStatistics()
        {
            var today = DateTime.Now.Date;
            var model = new AdminStatisticsViewModel() 
            {
                SiteVisit = db.SiteVisits.Where(s => s.CreateDate >= today ).Count(),
                Orders = db.Orders.Where(o => o.IsFinally == true && o.CreateDate >= today).Select(o => o.Amount).Sum(),
                ConfirmComment = db.BlogComments.Where(c => c.IsConfirm ==false && c.ParentId == null).Count(),
                AnswerComment = db.BlogComments.Where(c => c.IsAnswer ==false && c.ParentId == null).Count(),
                NewsAnswerComment = db.NewsComments.Where(c => c.IsAnswer == false && c.ParentId == null).Count(),
                NewsConfirmComment = db.NewsComments.Where(c => c.IsConfirm == false && c.ParentId == null).Count(),
                DownloadCount = db.SiteSettings.FirstOrDefault().DownloadCount
            };
            return model;
        }

        public string SendPassword(string username)
        {
            if (db.Admins.Any(a => a.UserName == username))
            {
                var admin = db.Admins.FirstOrDefault(a => a.UserName == username);
                Random random = new Random();
                string password = random.Next(1000,9999).ToString() + "_Twelve_"+random.Next(1000,9999);
                admin.Password = password;
                db.Admins.Update(admin);
                db.SaveChanges();

                return SendSms.SendPassword(admin.PhoneNumber, admin.Password).Result;
            }
            else
            {
                return "نام کاربری وارد شده معتبر نمی باشد";
            }
        }

        public string SendEmail(string username)
        {
            if (db.Admins.Any(a => a.UserName == username))
            {
                var admin = db.Admins.FirstOrDefault(a => a.UserName == username);
                Random random = new Random();
                string password = random.Next(1000, 9999).ToString() + "_Twelve_" + random.Next(1000, 9999);
                admin.Password = password;
                db.Admins.Update(admin);
                db.SaveChanges();

                try
                {
                    Utilities.SendEmail.Send(admin.Email, "رمز عبور دوازده", admin.Password);
                    return "رمز عبور شما با موفقیت به ایمیل شما ارسال شد";

                }
                catch 
                {
                    return "هنگام عملیات خطایی رخ داد لطفا مجددا تلاش فرمایید";

                }


            }
            else
            {
                return "نام کاربری وارد شده معتبر نمی باشد";
            }
        }

        public bool Verification(LoginViewModel login)
        {
            if (db.Admins.Any(a => a.UserName == login.username &&  a.Password == login.password))
            {
                return true;
            }
            else { return false; }
        }

        public bool Create(AdminCrudViewModel admin)
        {
            try
            {
                if (db.Admins.Any(a => a.UserName == admin.Username))
                {
                    return false;
                }
                Admin admins = new Admin() 
                {
                    AdminId = admin.AdminID,
                    Email = admin.Email,
                    Password = "123",
                    PhoneNumber = admin.Phonenumber,
                    UserName = admin.Username
                };
                db.Admins.Add(admins);
                db.SaveChanges();

                foreach (var item in admin.SelectedAccess)
                {
                    var selected = new SelectedAdminAccess() { AccessId = item, AdminId = admins.AdminId };
                    db.SelectedAdminAccesses.Add(selected);
                    db.SaveChanges();
                }
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public bool Update(AdminCrudViewModel admin)
        {
            try
            {
                if (db.Admins.Any(a => a.UserName == admin.Username && a.AdminId != admin.AdminID))
                {
                    return false;
                }
                Admin admins = db.Admins.Find(admin.AdminID);
                admins.UserName = admin.Username;
                admins.Email = admin.Email;
                admins.PhoneNumber = admin.Phonenumber;
                db.Admins.Update(admins);
                db.SaveChanges();

                db.SelectedAdminAccesses.Where(s => s.AdminId == admin.AdminID).ToList().ForEach(s => db.SelectedAdminAccesses.Remove(s));
                db.SaveChanges();

                foreach (var item in admin.SelectedAccess)
                {
                    var selected = new SelectedAdminAccess() { AdminId = admin.AdminID , AccessId = item};
                    db.SelectedAdminAccesses.Add(selected);
                    db.SaveChanges();
                }
                return true;
                
            }
            catch 
            {
                return false;
            }
        }

        public bool Delete(AdminCrudViewModel admin)
        {
            try
            {
                db.SelectedAdminAccesses.Where(s => s.AdminId == admin.AdminID).ToList().ForEach(s => db.SelectedAdminAccesses.Remove(s));
                db.SaveChanges();

                Admin admins = db.Admins.Find(admin.AdminID);
                db.Admins.Remove(admins);
                db.SaveChanges();

                

                return true;
            }
            catch 
            {
                return false;
            }
        }

        public AdminCrudViewModel GetByID(int id)
        {
            var admin = db.Admins.Find(id);

            AdminCrudViewModel model = new AdminCrudViewModel() 
            {
                AdminID = admin.AdminId,
                Username = admin.UserName,
                Phonenumber = admin.PhoneNumber,
                Email = admin.Email
            };
            var access = db.Acesses.ToList();
            model.Access = new List<AdminAccessViewModel>();
            foreach (var item in access)
            {
                model.Access.Add(new AdminAccessViewModel() 
                {
                    AccessID = item.AccessId,
                    AccessName = item.AccessText
                });
            }
            model.SelectedAccess = db.SelectedAdminAccesses.Where(a => a.AdminId == id).Select(a=> a.AccessId).ToList();
            return model;

        }

        public AdminCrudViewModel GetModelForCreate()
        {
            AdminCrudViewModel model = new AdminCrudViewModel();
            model.Access = new List<AdminAccessViewModel>();
            var access = db.Acesses.ToList();
            foreach (var item in access)
            {
                model.Access.Add(new AdminAccessViewModel() { AccessID = item.AccessId, AccessName = item.AccessText });
            }
            return model;
        }
    }
}
