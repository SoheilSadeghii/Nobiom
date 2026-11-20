using Microsoft.EntityFrameworkCore;
using Nobiom.Domain.Contracts;
using Nobiom.Domain.Models.Users;
using Nobiom.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nobiom.Infra.Data.Repositories
{
    public class UserRepositroy(EshopDbContext context) :
        IUserRepository
    {
        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await context.Users
                .SingleOrDefaultAsync(u => u.Id == userId);
        }

        public async Task CreateAsync(User user)
        {
            await context.Users.AddAsync(user);
        }

        public async Task DeleteAsync(User user)
        {
            user.IsDeleted = true;
            user.IsActive = false;
            user.DeleteDate = DateTime.Now;

            await UpdateAsync(user);
        }

        public async Task DeleteAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user != null) await DeleteAsync(userId);
        }

        public async Task UpdateAsync(User user)
        {
            context.Users.Update(user);
        }

        public async Task<bool> IsExistUserNameAsync(string userName)
        {
            return await context.Users.AnyAsync(u => u.UserName == userName);
        }

        public async Task<bool> IsExistUserNameAsync(string userName, int userId)
        {
            return await context.Users.AnyAsync(u =>
            u.UserName == userName &&
            u.Id != userId);
        }

        public async Task<bool> IsExistEmailAsync(string email)
        {
            return await context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> IsExistEmailAsync(string email, int userId)
        {
            return await context.Users.AnyAsync(u =>
            u.Email == email &&
            u.Id != userId);
        }

        public async Task<bool> IsExistMobileAsync(string mobile, int userId)
        {
            return await context.Users.AnyAsync(u =>
            u.Mobile == mobile &&
            u.Id != userId);
        }

        public async Task<bool> IsExistNationalCodeAsync(string nationalCode, int userId)
        {
            return await context.Users.AnyAsync(u =>
            u.NationalCode == nationalCode &&
            u.Id != userId);
        }

        public async Task<User?> GetUserByActiveCodeAsync(string activeCode)
        {
            return await context.Users.SingleOrDefaultAsync(u => u.EmailActiveCode == activeCode);
        }

        public async Task<User?> GetUserByUserNameOrEmailAsync(string userNameOrEmail)
        {
            return await context.Users
                .SingleOrDefaultAsync(u => u.UserName == userNameOrEmail || u.Email == userNameOrEmail);
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
