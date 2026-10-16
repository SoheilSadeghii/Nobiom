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
        Task<bool> IsExistEmailAsync(string email);

        Task<User?> GetUserByActiveCodeAsync(string activeCode);

        Task SaveAsync();

    }
}
