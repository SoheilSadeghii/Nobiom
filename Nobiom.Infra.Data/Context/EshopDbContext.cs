using Microsoft.EntityFrameworkCore;
using Nobiom.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nobiom.Infra.Data.Context
{
    public class EshopDbContext(DbContextOptions<EshopDbContext> options) :
        DbContext(options)
    {
        #region Users

        public DbSet<User> Users { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
