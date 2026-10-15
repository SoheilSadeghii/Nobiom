using Microsoft.AspNetCore.Mvc;
using Nobiom.Domain.ViewModels.Account;
using System.Runtime;

namespace Nobiom.Web.Controllers
{
    public class AccountController : Controller
    {
        #region Register

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            return View();
        }


        #endregion
    }
}
