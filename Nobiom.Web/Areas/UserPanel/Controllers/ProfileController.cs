using Microsoft.AspNetCore.Mvc;

namespace Nobiom.Web.Areas.UserPanel.Controllers
{
    public class ProfileController : UserPanelBaseController
    {
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }
    }
}
