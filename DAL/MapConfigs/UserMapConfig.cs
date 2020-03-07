using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.MapConfigs
{
    class UserMapConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Email).HasMaxLength(100).IsRequired();
            builder.HasIndex(u => u.Email).IsUnique(true);

            builder.Property(u => u.Password).HasMaxLength(100).IsRequired();
            
            builder.Property(u => u.Cpf).IsRequired();
            builder.HasIndex(u => u.Cpf).IsUnique(true);

            builder.Property(u => u.Rg).HasMaxLength(16).IsRequired();

            builder.Property(u => u.City).HasMaxLength(100).IsRequired();

            builder.Property(u => u.Street).HasMaxLength(120).IsRequired();

            builder.Property(u => u.Number).HasMaxLength(10).IsRequired();

            builder.Property(u => u.State).HasMaxLength(25).IsRequired();

            builder.Property(u => u.Name).HasMaxLength(200);
        }
    }
}
