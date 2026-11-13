using Microsoft.AspNetCore.Mvc;
using Nobiom.Application.Services.Interfaces;
using Nobiom.Application.Converter;
using Nobiom.Domain.ViewModels.Account;
using Nobiom.Application.Security;
using Nobiom.Application.Generators;
using Nobiom.Application.Extensions;

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

        #region Edit Profile

        public async Task<IActionResult> Index()
        {
            int currentUserId = int.Parse(User.GetUserId()!);

            var model = await _accountService.GetUserProfileAsync(currentUserId);

            if (model == null) return NotFound();

            if (model.Year.HasValue && model.Month.HasValue && model.Day.HasValue)
            {
                var birthDate = new DateTime(model.Year.Value, model.Month.Value, model.Day.Value);

                var shamsiDate = birthDate.ToShamsi();
                var parts = shamsiDate.Split('/');

                model.Year = int.Parse(parts[0]);
                model.Month = int.Parse(parts[1]);
                model.Day = int.Parse(parts[2]);
            }

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ProfileViewModel model, IFormFile? imgAvatar)
        {
            if (!ModelState.IsValid) return View(model);

            if (imgAvatar != null)
            {
                if (!imgAvatar.ImageValidate())
                {
                    ViewBag.ValidateImage = false;

                    return View(model);
                }

                if (model.Avatar != "default.png")
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Avatars", model.Avatar);

                    if (System.IO.File.Exists(deletePath)) System.IO.File.Delete(deletePath);

                }

                string avatarName = NameGenerator.GenerateUniqName() + Path.GetExtension(imgAvatar.FileName);

                string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Avatars", avatarName);
                using (var stream = System.IO.File.Create(savePath))
                {
                    imgAvatar.CopyTo(stream);
                }

                model.Avatar = avatarName;
            }

            int currentUserId = int.Parse(User.GetUserId()!);

            await _accountService.EditUserProfileAsync(model, currentUserId);

            TempData["SuccessEditProfile"] = "True";

            return Redirect("/UserPanel");
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
            int currentUserId = int.Parse(User.GetUserId()!);

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
