using ASPSnippets.Core.Captcha;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nobiom.Application.Services.Interfaces;
using Nobiom.Domain.Enums;
using Nobiom.Domain.ViewModels.Account;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Nobiom.Web.Controllers
{
    public class AccountController : Controller
    {

        #region Fields 

        private readonly IAccountService _accountService;


        #endregion

        #region Create Captcha

        public Captcha captcha
        {
            get
            {
                return JsonConvert.DeserializeObject<Captcha>(TempData["Captcha"]?.ToString() ?? "{}");
            }
            set
            {
                TempData["Captcha"] = JsonConvert.SerializeObject(value);
            }
        }

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
        public IActionResult Login(string ReturnUrl = "/")
        {
            captcha = new Captcha(90, 46, 22.5f, "#FFFFFF", "#2962ff", Mode.Arithmetic);

            ViewBag.ReturnUrl = ReturnUrl;

            return View(new LoginViewModel()
            {
                ImageData = this.captcha.ImageData
            });
        }

        [HttpPost("Login"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login, string ReturnUrl = "/")
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            if (!captcha.IsValid(login.CaptchaAnswer))
            {
                captcha = new Captcha(90, 46, 22.5f, "#FFFFFF", "#2962ff", Mode.Arithmetic);
                ModelState.AddModelError("CaptchaAnswer", "کد امنیتی وارد شده اشتباه است");

                login.ImageData = this.captcha.ImageData;

                return View(login);
            }

            var result = await _accountService.LoginAsync(login);

            if (result == LoginUserResult.NotFound)
            {
                captcha = new Captcha(90, 46, 22.5f, "#FFFFFF", "#2962ff", Mode.Arithmetic);

                login.ImageData = this.captcha.ImageData;

                ModelState.AddModelError("", "نام کاربری یا ایمیل وارد شده اشتباه است");

                return View(login);
            }

            if (result == LoginUserResult.NotActivated)
            {
                captcha = new Captcha(90, 46, 22.5f, "#FFFFFF", "#2962ff", Mode.Arithmetic);

                login.ImageData = this.captcha.ImageData;

                ViewBag.UserNotActivated = true;

                return View(login);
            }

            var user = await _accountService.GetUserByEmailOrUserNameAsync(login.UserNameOrEmail);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("FullName", $"{user.FirstName}:{user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email),
            };

            if (!string.IsNullOrWhiteSpace(user.Mobile))
            {
                claims.Add(new Claim(ClaimTypes.MobilePhone, user.Mobile));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties
            {
                IsPersistent = login.RememberMe
            };

            await HttpContext.SignInAsync(principal, properties);

            if (Url.IsLocalUrl(ReturnUrl)) return Redirect(ReturnUrl);

            return View("/");
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
