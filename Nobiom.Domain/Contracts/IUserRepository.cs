using Nobiom.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nobiom.Domain.Contracts
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUserAsync();
        Task<User?> GetUserByIdAsync(int userId);
        Task CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task DeleteAsync(int userId);

        Task<bool> IsExistUserNameAsync(string userName);
        Task<bool> IsExistUserNameAsync(string userName, int userId);
        Task<bool> IsExistEmailAsync(string email);
        Task<bool> IsExistEmailAsync(string email, int userId);
        Task<bool> IsExistMobileAsync(string mobile, int userId);
        Task<bool> IsExistNationalCodeAsync(string nationalCode, int userId);

        Task<User?> GetUserByActiveCodeAsync(string activeCode);

        Task<User?> GetUserByUserNameOrEmailAsync(string userNameOrEmail);

        Task SaveAsync();

    }
}
