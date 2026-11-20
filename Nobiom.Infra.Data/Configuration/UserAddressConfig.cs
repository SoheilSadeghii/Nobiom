using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nobiom.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nobiom.Infra.Data.Configuration
{
    public class UserAddressConfig : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            #region Key

            builder.HasKey(x => x.UserId);

            #endregion

            #region Validations 

            builder.Property(u => u.Title).IsRequired().HasMaxLength(300);
            builder.Property(u => u.PostalCode).IsRequired().HasMaxLength(300);
            builder.Property(u => u.Address).IsRequired().HasMaxLength(1500);

            #endregion

            #region

            builder.HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(u => u.UserId);

            #endregion
        }
    }
}
