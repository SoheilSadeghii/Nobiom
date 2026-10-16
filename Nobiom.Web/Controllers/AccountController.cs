using Microsoft.AspNetCore.Mvc;
using Nobiom.Application.Services.Interfaces;
using Nobiom.Domain.Enums;
using Nobiom.Domain.ViewModels.Account;
using System.Runtime;

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
    }
}
