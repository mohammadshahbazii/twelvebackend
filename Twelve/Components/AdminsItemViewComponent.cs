using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class AdminsItemViewComponent : ViewComponent
    {
        IAdminRepository adminRepository = new AdminRepository();
        public IViewComponentResult Invoke(string q = "", int PageID = 1)
        {
            var content = adminRepository.GetAdmins(q, PageID);


            return View("GetAdminsItem", content);
        }
    }
}
