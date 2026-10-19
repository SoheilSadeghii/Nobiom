using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Nobiom.Application.Services.Interfaces;
using Nobiom.Domain.Enums;
using Nobiom.Domain.ViewModels.Account;
using System.Runtime;
using System.Security.Claims;

namespace Nobiom.Web.Controllers
{
    public class AccountController : Controller
    {
        #region Fields 

        private readonly IAccountService _accountService;


        #endregion

        #region Constructor

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        #endregion

        #region Register

        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            var result = await _accountService.RegisterAsync(register);

            switch (result)
            {
                case RegisterUserResult.Success:
                    return View("SuccessRegister", register);

                case RegisterUserResult.EmailDuplicated:
                    ModelState.AddModelError("Email", "ایمیل وارد شده تکراری است");
                    break;

                case RegisterUserResult.UserNameDuplicated:
                    ModelState.AddModelError("UserName", "نام کاربری وارد شده تکراری است");
                    break;

                case RegisterUserResult.SendActivationEmail:
                    ModelState.AddModelError("Email", "ارسال ایمیل فعالسازی با مشکل مواجه شد، لطفا دقایقی دیگر تلاش کنید.");
                    break;

                case RegisterUserResult.InvalidInputs:
                    ModelState.AddModelError("", "لطفا فیلد های روی فرم را پر کنید");
                    break;

                case RegisterUserResult.Failed:
                    ModelState.AddModelError("", "خطای ناشناخته، لطفا به پشتیبانی اطلاع دهید");
                    break;
            }
            return View();
        }

        #endregion

        #region Activation Email

        [Route("VerifyEmail/{activeCode}")]
        public async Task<IActionResult> ActiveAccount(string activeCode)
        {
            ViewBag.ActiveAccount = await _accountService.ActiveAccountAsync(activeCode);


            return View();
        }

        #endregion

        #region Login

        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var result = await _accountService.LoginAsync(login);

            if (result == LoginUserResult.NotFound)
            {
                ModelState.AddModelError("", "نام کاربری یا ایمیل وارد شده اشتباه است");

                return View(login);
            }

            if (result == LoginUserResult.NotActivated)
            {
                ViewBag.UserNotActivated = true;

                return View(login);
            }

            var user = await _accountService.GetUserByEmailOrUserNameAsync(login.UserNameOrEmail);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("FullName",$"{user.FirstName}:{user.LastName}"),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties
            {
                IsPersistent = login.RememberMe
            };

            await HttpContext.SignInAsync(principal, properties);

            return Redirect("Home/Index");
        }

        #endregion

        #region SignOut

        [Route("Logout")]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/Login");
        }

        #endregion
    }
}
