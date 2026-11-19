using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Nobiom.Application.Converter;
using Nobiom.Application.Extensions;
using Nobiom.Application.Generators;
using Nobiom.Application.Security;
using Nobiom.Application.Services.Interfaces;
using Nobiom.Domain.Enums;
using Nobiom.Domain.Models.Users;
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
            if (!ModelState.IsValid)
                return View(model);

            int currentUserId = int.Parse(User.GetUserId()!);

            string? newAvatarPath = null;
            string oldAvatar = model.Avatar;

            #region Avatar

            if (imgAvatar != null)
            {
                if (!imgAvatar.ImageValidate())
                {
                    ViewBag.ValidateImage = false;
                    return View(model);
                }

                string avatarName = NameGenerator.GenerateUniqName()
                                    + Path.GetExtension(imgAvatar.FileName);

                newAvatarPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "Avatars",
                    avatarName);

                try
                {
                    await using (var stream = new FileStream(
                        newAvatarPath,
                        FileMode.CreateNew,
                        FileAccess.Write,
                        FileShare.None))
                    {
                        await imgAvatar.CopyToAsync(stream);
                    }

                    model.Avatar = avatarName;
                }
                catch
                {
                    if (System.IO.File.Exists(newAvatarPath))
                        System.IO.File.Delete(newAvatarPath);

                    throw;
                }
            }

            #endregion

            var result = await _accountService.EditUserProfileAsync(model, currentUserId);

            switch (result)
            {
                case EditProfileResult.Success:

                    // Delete old avatar only after successful profile update.
                    if (newAvatarPath != null && oldAvatar != "default.png")
                    {
                        string oldAvatarPath = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot",
                            "Avatars",
                            oldAvatar);

                        if (System.IO.File.Exists(oldAvatarPath))
                            System.IO.File.Delete(oldAvatarPath);
                    }

                    TempData["SuccessEditProfile"] = "True";
                    return Redirect("/UserPanel");

                case EditProfileResult.EmailVerificationRequired:

                    await HttpContext.SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    var register = new ProfileViewModel
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email
                    };

                    ViewBag.Subject = "به دلیل تغییر ایمیل نیاز به فعالسازی ایمیل مجدد هست";

                    return View("SuccessEditEmail", register);

                case EditProfileResult.NoChanges:
                    return Redirect("/UserPanel");

                case EditProfileResult.UserNotFound:
                    break;

                case EditProfileResult.InvalidInputs:
                    ModelState.AddModelError(
                        string.Empty,
                        "لطفاً همه فیلدهای الزامی را تکمیل کنید.");
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

            // Profile update failed: remove the newly uploaded avatar.
            if (newAvatarPath != null)
            {
                if (System.IO.File.Exists(newAvatarPath))
                    System.IO.File.Delete(newAvatarPath);

                // Restore the previous avatar filename
                model.Avatar = oldAvatar;
            }

            if (result == EditProfileResult.UserNotFound)
                return NotFound();

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
