using Microsoft.AspNetCore.Mvc;
using Nobiom.Application.Converter;
using Nobiom.Application.Extensions;
using Nobiom.Application.Generators;
using Nobiom.Application.Security;
using Nobiom.Application.Services.Interfaces;
using Nobiom.Domain.Enums;
using Nobiom.Domain.ViewModels.Account;

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

            #region Avatar

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

            #endregion

            int currentUserId = int.Parse(User.GetUserId()!);

            var result = await _accountService.EditUserProfileAsync(model, currentUserId);

            switch (result)
            {
                case EditProfileResult.Success:
                    TempData["SuccessEditProfile"] = "True";
                    return Redirect("/UserPanel");

                case EditProfileResult.UserNotFound:
                    return NotFound();

                case EditProfileResult.InvalidInputs:
                    ModelState.AddModelError(string.Empty, "لطفاً همه فیلدهای الزامی را تکمیل کنید.");
                    break;

                case EditProfileResult.DuplicateUserName:
                    ModelState.AddModelError(
                        nameof(model.UserName),
                        "این نام کاربری قبلاً ثبت شده است.");
                    break;

                case EditProfileResult.DuplicateMobile:
                    ModelState.AddModelError(
                        nameof(model.Mobile),
                        "این شماره موبایل قبلاً ثبت شده است.");
                    break;

                case EditProfileResult.DuplicateEmail:
                    ModelState.AddModelError(
                        nameof(model.Email),
                        "این ایمیل قبلاً ثبت شده است.");
                    break;

                case EditProfileResult.DuplicateNationalCode:
                    ModelState.AddModelError(
                        nameof(model.NationalCode),
                        "این کد ملی قبلاً ثبت شده است.");
                    break;
            }

            return View(model);
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
