using Nobiom.Application.TextFixer;
using Nobiom.Application.Generators;
using Nobiom.Application.Mapper;
using Nobiom.Application.Services.Interfaces;
using Nobiom.Domain.Contracts;
using Nobiom.Domain.Enums;
using Nobiom.Application.Security;
using Nobiom.Domain.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Text;
using Nobiom.Domain.Models.Users;

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
    }
}
