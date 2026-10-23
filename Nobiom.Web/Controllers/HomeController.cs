using Microsoft.AspNetCore.Mvc;
using Nobiom.Application.Security;
using Nobiom.Infra.Data.Context;
using System;

namespace Nobiom.Web.Controllers
{
    public class HomeController : Controller
    {
        //private readonly EshopDbContext _context;
        //public HomeController(EshopDbContext context)
        //{
        //    _context = context;
        //    var user = context.Users.FirstOrDefault();
        //    user.Password = PasswordHasher.EncodePasswordMd5("123");
        //    _context.SaveChanges();
        //}
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
