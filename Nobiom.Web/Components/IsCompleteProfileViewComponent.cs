using Microsoft.AspNetCore.Mvc;
using Nobiom.Application.Extensions;
using Nobiom.Application.Services.Interfaces;

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
            int userId = int.Parse(HttpContext.User.GetUserId()!);

            bool model = await _accountService.IsCompleteProfileAsync(userId);

            return View(model);
        }
    }
}
