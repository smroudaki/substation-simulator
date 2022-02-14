using ElectricalEmulator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricalEmulator.Infra.Data.Persistence.Configurations
{
    public class UserClassConfiguration : IEntityTypeConfiguration<UserClass>
    {
        public void Configure(EntityTypeBuilder<UserClass> builder)
        {
            builder.HasIndex(b => b.ClassId);

            builder.HasIndex(b => b.UserId);

            builder.Property(b => b.CreationDate)
                .HasDefaultValueSql("(getdate())");

            builder.Property(b => b.IsAccept)
                .HasDefaultValueSql("((0))");

            builder.Property(e => e.UserClassGuid)
                .HasColumnType("UNIQUEIDENTIFIER ROWGUIDCOL")
                .HasDefaultValueSql("(newid())");

            builder.Property(b => b.UserId)
                .IsRequired();

            builder.HasOne(uc => uc.Class)
                .WithMany(c => c.UserClass)
                .HasForeignKey(uc => uc.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserClass_Class");

            builder.HasOne(uc => uc.User)
                .WithMany(u => u.UserClass)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserClass_User");
        }
    }
}
