using Microsoft.AspNetCore.Mvc;
using Nobiom.Application.Services.Interfaces;
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

            return View();
        }


        #endregion
    }
}
