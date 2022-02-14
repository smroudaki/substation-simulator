using ElectricalEmulator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricalEmulator.Infra.Data.Persistence.Configurations
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.HasIndex(b => b.ClassId);

            builder.HasIndex(b => b.PostId);

            builder.HasIndex(b => b.UserId);

            builder.Property(b => b.CreationDate)
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.ReportGuid)
                .HasColumnType("UNIQUEIDENTIFIER ROWGUIDCOL")
                .HasDefaultValueSql("(newid())");

            builder.Property(b => b.PostId)
                .IsRequired();

            builder.Property(b => b.UserId)
                .IsRequired();

            builder.Property(b => b.Value)
                .IsRequired();

            builder.HasOne(r => r.Class)
                .WithMany(c => c.Report)
                .HasForeignKey(r => r.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Report_Class");

            builder.HasOne(r => r.Post)
                .WithMany(p => p.Report)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Report_Post");

            builder.HasOne(r => r.User)
                .WithMany(u => u.Report)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Report_User");
        }
    }
}
