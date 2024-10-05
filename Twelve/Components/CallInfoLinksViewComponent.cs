using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class CallInfoLinksViewComponent : ViewComponent
    {
        ICallInfoRepository callInfoRepository = new CallInfoRepository();
        public IViewComponentResult Invoke(int CallInfoID)
        {
            var content = callInfoRepository.GetCallInfoLinks(CallInfoID);
            return View("GetCallInfoLinks",content);
        }
    }
}
