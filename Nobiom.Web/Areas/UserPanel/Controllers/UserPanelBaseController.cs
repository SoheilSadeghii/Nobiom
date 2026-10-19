using Microsoft.AspNetCore.Mvc;

namespace Nobiom.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    public class UserPanelBaseController : Controller
    {
        public IActionResult Index()
        {
            return Content("Hello from UserPanel!");
        }
    }
}
