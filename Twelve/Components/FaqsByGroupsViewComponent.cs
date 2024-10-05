using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class FaqsByGroupsViewComponent : ViewComponent
    {
        IFaqsRepository faqsRepository = new FaqsRepository();
        public IViewComponentResult Invoke(int GroupID)
        {
            var content = faqsRepository.GetFaqsByGroups(GroupID);
            return View("GetFaqsByGroups", content);
        }
    }
}
