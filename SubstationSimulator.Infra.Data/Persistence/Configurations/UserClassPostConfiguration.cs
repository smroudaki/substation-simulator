using ElectricalEmulator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricalEmulator.Infra.Data.Persistence.Configurations
{
    public class UserClassPostConfiguration : IEntityTypeConfiguration<UserClassPost>
    {
        public void Configure(EntityTypeBuilder<UserClassPost> builder)
        {
            builder.HasIndex(b => b.PostId);

            builder.HasIndex(b => b.UserClassId);

            builder.Property(b => b.CreationDate)
                .HasDefaultValueSql("(getdate())");

            builder.Property(b => b.PostChanges)
               .IsRequired();

            builder.Property(e => e.UserClassPostGuid)
                .HasColumnType("UNIQUEIDENTIFIER ROWGUIDCOL")
                .HasDefaultValueSql("(newid())");

            builder.Property(b => b.UserClassId)
                .IsRequired();

            builder.HasOne(up => up.Post)
                .WithMany(p => p.UserClassPost)
                .HasForeignKey(up => up.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserClassPost_Post");

            builder.HasOne(up => up.UserClass)
                .WithMany(u => u.UserClassPost)
                .HasForeignKey(up => up.UserClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserClassPost_UserClass");
        }
    }
}
