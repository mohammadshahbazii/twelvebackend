using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ViewModels;

namespace Twelve.Components
{
    public class IndexAboutContentViewComponent : ViewComponent
    {
        private readonly IStringLocalizer<IndexAboutContentViewComponent> _localizer;

        public IndexAboutContentViewComponent(IStringLocalizer<IndexAboutContentViewComponent> localizer)
        {
            _localizer = localizer;
        }

        public IViewComponentResult Invoke()
        {
            var content = new ContentItemViewModel
            {
                Title = _localizer["AboutTitle"],
                SubTitle = _localizer["AboutSubTitle"],
                Description = _localizer["AboutDescription"]
            };
            return View("GetIndexAboutContent", content);
        }
    }
}
