using Microsoft.AspNetCore.Mvc;
using Nobiom.Application.Services.Interfaces;
using Nobiom.Domain.Enums;
using Nobiom.Domain.ViewModels.Account;
using System.Security.Claims;

namespace Nobiom.Web.Areas.UserPanel.Controllers
{
    public class ProfileController : UserPanelBaseController
    {
        #region Fields

        private readonly IAccountService _accountService;

        #endregion

        #region Constructors

        public ProfileController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        #endregion

        #region Change Password

        [Route("Change-Password")]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        [HttpPost("Change-Password"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var result = await _accountService.ChangeUserPassword(model, currentUserId);

            if (!result)
            {
                ModelState.AddModelError("OldPassword", "کلمه عبور صحیح نمی باشد");
                return View(model);
            }

            return Redirect("/Logout");
        }

        #endregion
    }
}
