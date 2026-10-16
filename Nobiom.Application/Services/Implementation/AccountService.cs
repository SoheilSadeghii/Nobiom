using Nobiom.Application.Extensions;
using Nobiom.Application.Services.Interfaces;
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

            if (await _userRepository.IsExistUserNameAsync(model.UserName.FixEmail()))
            {
                return RegisterUserResult.UserNameDuplicated;
            }

            #endregion

            User user = new User()
            {
                UserName = model.UserName.FixUserName(),
                Email = model.Email.FixEmail(),
                Avatar = "NoPhoto.jpg",
                CreateDate = DateTime.Now,
                EmailActiveCode = Guid.NewGuid().ToString().Replace("-", ""),
                IsActive = true,
                IsEmailActive = false,
                Password = model.Password
            };

            await _userRepository.CreateAsync(user);
            await _userRepository.SaveAsync();

            return RegisterUserResult.Success;
        }
    }
}
