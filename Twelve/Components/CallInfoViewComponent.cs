using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class CallInfoViewComponent : ViewComponent
    {
        ICallInfoRepository callInfoRepository = new CallInfoRepository();
        public IViewComponentResult Invoke()
        {
            var content = callInfoRepository.GetCallInfoes();
            return View("GetCallInfo",content);
        }
    }
}
