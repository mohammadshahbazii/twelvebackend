using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class ContactsFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("GetContactsForm");
        }
    }
}
