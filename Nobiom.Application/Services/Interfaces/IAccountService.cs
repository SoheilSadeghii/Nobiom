using Nobiom.Domain.Enums;
using Nobiom.Domain.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nobiom.Application.Services.Interfaces
{
    public interface IAccountService
    {
        Task<RegisterUserResult> RegisterAsync(RegisterViewModel model);
        Task<bool> ActiveAccountAsync(string activeCode);
    }
}
