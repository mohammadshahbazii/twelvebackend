using Microsoft.AspNetCore.Mvc;
using Services;
using ZarinpalSandbox;

namespace Twelve.Controllers
{
    public class OrdersController : Controller
    {
        IOrderRepository orderRepository = new OrderRepository();
        public IActionResult Index()
        {
            return View();
        }

        [Route("Payment")]
        public IActionResult Payment(int Amount)
        {
            var payment = new Payment(Amount/10);
            int orderID = orderRepository.CreateOrder(Amount);
            var res = payment.PaymentRequest("حمایت از سوپر اپلیکیشن دوازده", "https://localhost:7050/PaymentResult/"+orderID);

            if (res.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority);
            }
            else
            {
                return View("Error");
            }
        }

        [Route("PaymentResult/{OrderID}")]
        public IActionResult PaymentResult(int OrderID)
        {
            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok"
                && HttpContext.Request.Query["Authority"] != "")
            {
                orderRepository.FinalizedOrder(OrderID);
                string message = "تراکنش شما با موفقیت انجام شد . با سپاس از لطف بی کران شما";
                HttpContext.Session.SetString("PaymentMessage", message);
                return RedirectToAction("ContactUs","Home");
            }
            else 
            {
                string message = "کاربر گرامی! هنگام تراکنش خطایی رخ داد لطفا مجددا تلاش فرمایید . با سپاس از لطف بی کران شما";
                HttpContext.Session.SetString("PaymentMessage", message);
                return RedirectToAction("ContactUs", "Home");
            }
        }

    }
}
