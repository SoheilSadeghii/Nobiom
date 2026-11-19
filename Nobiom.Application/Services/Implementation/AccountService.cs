using Nobiom.Application.Converter;
using Nobiom.Application.Generators;
using Nobiom.Application.Mapper;
using Nobiom.Application.Security;
using Nobiom.Application.Services.Interfaces;
using Nobiom.Application.TextFixer;
using Nobiom.Domain.Contracts;
using Nobiom.Domain.Enums;
using Nobiom.Domain.Models.Users;
using Nobiom.Domain.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nobiom.Application.Services.Implementation
{
    public class AccountService(IUserRepository _userRepository)
        : IAccountService
    {

        public async Task<RegisterUserResult> RegisterAsync(RegisterViewModel model)
        {
            #region Validations

            if (string.IsNullOrWhiteSpace(model.UserName) ||
                string.IsNullOrWhiteSpace(model.Email) ||
                string.IsNullOrWhiteSpace(model.Password))
            {
                return RegisterUserResult.InvalidInputs;
            }

            if (await _userRepository.IsExistEmailAsync(model.Email.FixEmail()))
            {
                return RegisterUserResult.EmailDuplicated;
            }

            if (await _userRepository.IsExistUserNameAsync(model.UserName.FixUserName()))
            {
                return RegisterUserResult.UserNameDuplicated;
            }

            #endregion

            var user = UserMapper.MapToUser(model);

            await _userRepository.CreateAsync(user);
            await _userRepository.SaveAsync();

            return RegisterUserResult.Success;
        }

        public async Task<bool> ActiveAccountAsync(string activeCode)
        {
            var user = await _userRepository.GetUserByActiveCodeAsync(activeCode);

            if (user == null) return false;

            user.IsEmailActive = true;
            user.EmailActiveCode = NameGenerator.GenerateUniqName();

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveAsync();

            return true;
        }

        public async Task<LoginUserResult> LoginAsync(LoginViewModel model)
        {
            var user = await _userRepository.GetUserByUserNameOrEmailAsync(model.UserNameOrEmail);

            if (user == null) return LoginUserResult.NotFound;

            if (user.IsDeleted) return LoginUserResult.NotFound;

            if (!PasswordHasher.VerifyPassword(model.Password, user.Password)) return LoginUserResult.NotFound;

            if (!user.IsEmailActive) return LoginUserResult.NotActivated;

            return LoginUserResult.Success;
        }

        public async Task<User?> GetUserByEmailOrUserNameAsync(string emailOrUsername)
        {
            return await _userRepository.GetUserByUserNameOrEmailAsync(emailOrUsername);
        }

        public async Task<bool> ChangeUserPassword(ChangePasswordViewModel model, int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null) throw new Exception("کاربر یافت نشد");

            if (!PasswordHasher.VerifyPassword(model.OldPassword, user.Password)) return false;

            user.Password = PasswordHasher.EncodePasswordMd5(model.Password);

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveAsync();

            return true;
        }

        public async Task<ProfileViewModel?> GetUserProfileAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null) return null;

            return new ProfileViewModel()
            {
                Avatar = user.Avatar,
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty,
                Mobile = user.Mobile ?? string.Empty,
                NationalCode = user.NationalCode,
                Year = user.BirthDate?.Year,
                Month = user.BirthDate?.Month,
                Day = user.BirthDate?.Day,
                Email = user.Email,
                UserName = user.UserName
            };
        }

        public async Task<bool> IsCompleteProfileAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null) return false;

            if (user.Mobile != null && user.Mobile != "")
            {
                return true;
            }

            return false;
        }

        public async Task<EditProfileResult> EditUserProfileAsync(ProfileViewModel model, int userId)
        {
            #region Validation

            if (string.IsNullOrWhiteSpace(model.UserName) ||
                string.IsNullOrWhiteSpace(model.Email) ||
                string.IsNullOrWhiteSpace(model.Mobile))
            {
                return EditProfileResult.InvalidInputs;
            }

            if (await _userRepository.IsExistEmailAsync(model.Email.FixEmail(), userId)) return EditProfileResult.DuplicateEmail;

            if (await _userRepository.IsExistMobileAsync(model.Mobile, userId)) return EditProfileResult.DuplicateMobile;

            if (!string.IsNullOrWhiteSpace(model.NationalCode) &&
                await _userRepository.IsExistNationalCodeAsync(model.NationalCode, userId))
                return EditProfileResult.DuplicateNationalCode;

            if (await _userRepository.IsExistUserNameAsync(model.UserName.FixUserName(), userId)) return EditProfileResult.DuplicateUserName;

            #endregion

            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null) return EditProfileResult.UserNotFound;

            var normalizedUserName = model.UserName.FixUserName();
            var normalizedEmail = model.Email.FixEmail();

            bool emailChanged = !string.Equals(
                user.Email,
                normalizedEmail,
                StringComparison.OrdinalIgnoreCase);

            DateOnly? submittedBirthDate = null;

            if (model.Year.HasValue &&
                model.Month.HasValue &&
                model.Day.HasValue)
            {
                var shamsiDate = $"{model.Year}/{model.Month:00}/{model.Day:00}";
                submittedBirthDate = DateOnly.FromDateTime(shamsiDate.ToMiladi());
            }

            bool hasChanges =
                user.FirstName != model.FirstName ||
                user.LastName != model.LastName ||
                user.UserName != normalizedUserName ||
                user.Mobile != model.Mobile ||
                user.NationalCode != model.NationalCode ||
                emailChanged ||
                user.Avatar != model.Avatar ||
                user.BirthDate != submittedBirthDate;

            if (!hasChanges)
                return EditProfileResult.NoChanges;
                        
            if (emailChanged)
            {
                if (await _userRepository.IsExistEmailAsync(normalizedEmail, userId))
                    return EditProfileResult.DuplicateEmail;

                user.Email = normalizedEmail;
                user.IsEmailActive = false;
                user.EmailActiveCode = NameGenerator.GenerateUniqName();
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.UserName.FixUserName();
            user.Mobile = model.Mobile;
            user.NationalCode = model.NationalCode;
            user.Mobile = model.Mobile;
            user.Email = model.Email.FixEmail();
            user.Avatar = model.Avatar;

            if (model.Year.HasValue &&
                model.Month.HasValue &&
                model.Day.HasValue)
            {
                var shamsiDate = $"{model.Year}/{model.Month:00}/{model.Day:00}";

                user.BirthDate = DateOnly.FromDateTime(shamsiDate.ToMiladi());
            }

            await _userRepository.UpdateAsync(user);

            await _userRepository.SaveAsync();

            return emailChanged
                ? EditProfileResult.EmailVerificationRequired
                : EditProfileResult.Success;
        }
    }
}
