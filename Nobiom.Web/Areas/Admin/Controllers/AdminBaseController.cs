using Microsoft.AspNetCore.Mvc;

namespace Nobiom.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminBaseController : Controller
    {
        public IActionResult Index()
        {
            return Content("Hello from Admin!");
        }
    }
}
