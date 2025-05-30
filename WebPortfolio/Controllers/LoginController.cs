using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebPortfolio.Models.Data;
using WebPortfolio.Models.Entities;
using System.Net.Mail;
using System.Net;

namespace WebPortfolio.Controllers
{
    public class LoginController : Controller
    {
        Context context = new Context();

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Admin");
            }

            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(Admin admin)
        {
            var info = context.Admins.FirstOrDefault(x => x.Username == admin.Username && x.Password == admin.Password);
            if (info != null)
            {
                // 6 rəqəmli təsdiq kodu yaradılır
                var verificationCode = new Random().Next(100000, 999999).ToString();

                // Kodun və istifadəçi adının müvəqqəti saxlanılması
                TempData["VerificationCode"] = verificationCode;
                TempData["Username"] = info.Username;

                // Gmail-ə göndər
                SendVerificationCode(info.Email, verificationCode);

                return RedirectToAction("VerifyCode");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Username or Password are incorrect");
                return View();
            }
        }

        public IActionResult VerifyCode()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Admin");
            }

            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return View();
        }




        [HttpPost]
        public async Task<IActionResult> VerifyCode(string code)
        {
            var expectedCode = TempData["VerificationCode"] as string;
            var username = TempData["Username"] as string;

            // Bu sətri əlavə et — TempData-nı növbəti sorğuda da saxlayır
            TempData.Keep();

            if (code == expectedCode && !string.IsNullOrEmpty(username))
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username)
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = false
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );

                return RedirectToAction("Index", "Admin");
            }

            ModelState.AddModelError(string.Empty, "Verification code is incorrect");
            return View();
        }


        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }

        private void SendVerificationCode(string toEmail, string code)
        {
            var fromAddress = new MailAddress("turgut.nitro17@gmail.com", "YourAppName");
            var toAddress = new MailAddress(toEmail);
            const string fromPassword = "dnsn dcer bryn zucn"; // Gmail üçün app password istifadə et
            const string subject = "Your verification code";
            string body = $"Your verification code is: {code}";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };
            smtp.Send(message);
        }
    }
}
