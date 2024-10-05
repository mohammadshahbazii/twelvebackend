using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Twelve.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/Orders")]
    public class OrdersController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();
        IOrderRepository orderRepository = new OrderRepository();

        public IActionResult Index()
        {
            if (adminRepository.CheckPermission(User.Identity.Name, "لیست تراکنش های مالی"))
            {
                var content = orderRepository.GetOrders();
                return View(content);
            }
            else
            {
                return View("Error");
            }
        }

        [Route("GetOrders")]
        public IActionResult GetOrders( int PageID = 1)
        {
            return ViewComponent("AdminOrdersItem", new { PageID = PageID });
        }
    }
}
