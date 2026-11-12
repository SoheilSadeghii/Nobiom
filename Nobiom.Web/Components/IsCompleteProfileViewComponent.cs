using Microsoft.AspNetCore.Mvc;
using Nobiom.Application.Services.Interfaces;
using System.Security.Claims;

namespace Nobiom.Web.Components
{
    public class IsCompleteProfileViewComponent : ViewComponent
    {
        #region Fields

        private readonly IAccountService _accountService;

        #endregion

        #region Constructors

        public IsCompleteProfileViewComponent(IAccountService accountService)
        {
            _accountService = accountService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {    
            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());

            bool model = await _accountService.IsCompleteProfileAsync(userId);

            return View(model);
        }
    }
}
