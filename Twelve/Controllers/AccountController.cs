using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Security.Claims;
using ViewModels;

namespace Twelve.Controllers
{
    public class AccountController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();

        public IActionResult Index()
        {
            return View();
        }

        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginViewModel login, string ReturnUrl = "/")
        {

            if (adminRepository.Verification(login))
            {
                var admin = adminRepository.GetAdminByUsername(login.username);
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,admin.AdminId.ToString()),
                    new Claim(ClaimTypes.Name,login.username)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties
                {

                    IsPersistent = true,

                };
                HttpContext.SignInAsync(principal, properties);
                ViewBag.IsSuccess = true;
                return Redirect(ReturnUrl);
            }
            else
            {
                ViewBag.Message = "مشخصات وارد شده معتبر نمی باشد";
                return View();
            }
        }

        [Route("LogOut")]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        [Route("SenPassword")]
        public string SenPassword(string username)
        {
            return adminRepository.SendPassword(username);
        }

        [Route("SendEmail")]
        public string SendEmail(string username)
        {
            return adminRepository.SendEmail(username);
        }
    }
}
