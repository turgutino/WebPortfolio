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
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
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
                
                var verificationCode = new Random().Next(100000, 999999).ToString();

            
                TempData["VerificationCode"] = verificationCode;
                TempData["Username"] = info.Username;

             
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
            var fromAddress = new MailAddress("", "YourAppName"); //Burada boş hissəyə öz emailinizi yazın
            var toAddress = new MailAddress(toEmail);
            const string fromPassword = "";  //Buraya ise emailiniz ucun yaratdiginin 16 reqemli xususi parolu yazin
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

        public IActionResult ForgotPassword()
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            var user = context.Admins.FirstOrDefault(x => x.Email == email);
            if (user != null)
            {
                var resetCode = new Random().Next(100000, 999999).ToString();
                TempData["ResetCode"] = resetCode;
                TempData["ResetEmail"] = email;
                SendVerificationCode(email, resetCode);
                return RedirectToAction("VerifyResetCode");
            }

            ModelState.AddModelError("", "Email tapılmadı.");
            return View();
        }

        public IActionResult VerifyResetCode()
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return View();
        }

        [HttpPost]
        public IActionResult VerifyResetCode(string code)
        {
            var expectedCode = TempData["ResetCode"] as string;
            var email = TempData["ResetEmail"] as string;

            TempData.Keep(); 

            if (code == expectedCode)
            {
                TempData["AllowReset"] = true;
                return RedirectToAction("ResetPassword");
            }

            ModelState.AddModelError("", "Kod yanlışdır.");
            return View();
        }

        public IActionResult ResetPassword()
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            if (TempData["AllowReset"] == null || !(bool)TempData["AllowReset"])
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(string newPassword)
        {
            var email = TempData["ResetEmail"] as string;
            var user = context.Admins.FirstOrDefault(x => x.Email == email);

            if (user != null)
            {
                user.Password = newPassword;
                context.SaveChanges();
                return RedirectToAction("Index", "Login");
            }

            ModelState.AddModelError("", "Xəta baş verdi.");
            return View();
        }


    }
}
