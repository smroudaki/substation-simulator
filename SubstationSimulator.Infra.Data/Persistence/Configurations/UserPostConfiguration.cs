using ElectricalEmulator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricalEmulator.Infra.Data.Persistence.Configurations
{
    public class UserPostConfiguration : IEntityTypeConfiguration<UserPost>
    {
        public void Configure(EntityTypeBuilder<UserPost> builder)
        {
            builder.HasIndex(b => b.PostId);

            builder.HasIndex(b => b.UserId);

            builder.Property(b => b.CreationDate)
                .HasDefaultValueSql("(getdate())");

            builder.Property(b => b.PostChanges)
               .IsRequired();

            builder.Property(e => e.UserPostGuid)
                .HasColumnType("UNIQUEIDENTIFIER ROWGUIDCOL")
                .HasDefaultValueSql("(newid())");

            builder.Property(b => b.UserId)
                .IsRequired();

            builder.HasOne(up => up.Post)
                .WithMany(p => p.UserPost)
                .HasForeignKey(up => up.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPost_Post");

            builder.HasOne(up => up.User)
                .WithMany(u => u.UserPost)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPost_User");
        }
    }
}
