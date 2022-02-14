using ElectricalEmulator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ElectricalEmulator.Infra.Data.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(b => b.CreationDate)
                .HasDefaultValueSql("(getdate())");

            builder.Property(b => b.FirstName)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(b => b.IsActive)
                .HasDefaultValueSql("((1))");

            builder.Property(b => b.LastName)
                .IsRequired()
                .HasMaxLength(128);
        }
    }
}
