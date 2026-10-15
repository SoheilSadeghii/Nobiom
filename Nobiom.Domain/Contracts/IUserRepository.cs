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
        Task SaveAsync();

    }
}
