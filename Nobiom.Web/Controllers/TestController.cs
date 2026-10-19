using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Nobiom.Web.Controllers
{
    [Authorize]
    public class TestController : Controller
    {
        public IActionResult Index1()
        {
            return Content("Hello, Index1!");
        }

        public IActionResult Index2()
        {
            return Content("Hello, Index2!");
        }
    }
}
