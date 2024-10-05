using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Components
{
    public class AdminOrdersItemViewComponent : ViewComponent
    {
        IOrderRepository orderRepository = new OrderRepository();
        public IViewComponentResult Invoke(int PageID = 1)
        {
            var content = orderRepository.GetOrders(PageID);


            return View("GetAdminOrdersItem", content);
        }
    }
}
