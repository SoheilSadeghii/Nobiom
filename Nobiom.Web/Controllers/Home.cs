using Microsoft.AspNetCore.Mvc;
using System;

namespace Nobiom.Web.Controllers
{
    [Mapco]
    public class Home : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("About-Us")]
        public IActionResult AboutUs()
        {
            return View();
        }

        [Route("Contact-Us")]
        public IActionResult ContactUs()
        {
            return View();
        }
    }
}

namespace Nobiom.Web
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class MapcoAttribute : Attribute
    {
        public MapcoAttribute() { }
        // add properties/ctors here if you need configuration
    }
}
