using Nobiom.Application.TextFixer;
using Nobiom.Application.Generators;
using Nobiom.Application.Security;
using Nobiom.Domain.Models.Users;
using Nobiom.Domain.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nobiom.Application.Mapper
{
    public static class UserMapper
    {
        public static User MapToUser(RegisterViewModel model)
        {
            return new User()
            {
                UserName = model.UserName.FixUserName(),
                Email = model.Email.FixEmail(),
                Avatar = "default.png",
                CreateDate = DateTime.Now,
                EmailActiveCode = NameGenerator.GenerateUniqName(),
                IsActive = true,
                IsEmailActive = false,
                Password = PasswordHasher.EncodePasswordMd5(model.Password),

            };
        }
    }
}
