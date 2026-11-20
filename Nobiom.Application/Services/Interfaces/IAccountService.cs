using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Nobiom.Domain.Enums;
using Nobiom.Domain.Models.Users;
using Nobiom.Domain.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Nobiom.Application.Services.Interfaces
{
    public interface IAccountService
    {
        Task<RegisterUserResult> RegisterAsync(RegisterViewModel model);
        Task<LoginUserResult> LoginAsync(LoginViewModel model);
        Task<bool> ActiveAccountAsync(string activeCode);
        Task<User?> GetUserByEmailOrUserNameAsync(string emailOrUsername);
        Task<bool> ChangeUserPassword(ChangePasswordViewModel model, int userId);
        Task<ProfileViewModel?> GetUserProfileAsync(int userId);
        Task<bool> IsCompleteProfileAsync(int userId);
        Task<EditProfileResult> EditUserProfileAsync(ProfileViewModel model, int userId);
        ClaimsPrincipal CreateAuthenticationPrincipal(User user);
    }
}
