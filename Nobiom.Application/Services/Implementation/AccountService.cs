using Nobiom.Application.Services.Interfaces;
using Nobiom.Domain.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nobiom.Application.Services.Implementation
{
    public class AccountService(IUserService _userRepository)
        : IAccountService
    {
        public Task<bool> RegisterAsync(RegisterViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
