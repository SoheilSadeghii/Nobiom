using Microsoft.AspNetCore.Mvc;
using Nobiom.Domain.ViewModels.Account;

namespace Nobiom.Web.Areas.UserPanel.Controllers
{
    public class ProfileController : UserPanelBaseController
    {
        #region Change Password

        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            return View();
        }

        #endregion
    }
}
