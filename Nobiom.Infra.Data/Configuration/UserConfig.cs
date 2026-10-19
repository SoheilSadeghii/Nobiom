using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nobiom.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nobiom.Infra.Data.Configuration
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            #region Key

            builder.HasKey(u => u.Id);

            #endregion

            #region Validations

            builder.Property(u => u.FirstName).HasMaxLength(200);
            builder.Property(u => u.LastName).HasMaxLength(200);
            builder.Property(u => u.UserName).IsRequired().HasMaxLength(200);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
            builder.Property(u => u.Password).IsRequired().HasMaxLength(400);
            builder.Property(u => u.EmailActiveCode).HasMaxLength(50);

            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.UserName).IsUnique();
            builder.HasIndex(u => u.Mobile).IsUnique();

            #endregion

            #region Relaitions

            #region Relations

            builder.HasMany(u => u.Addresses)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);

            #endregion

            #endregion
        }
    }
}
