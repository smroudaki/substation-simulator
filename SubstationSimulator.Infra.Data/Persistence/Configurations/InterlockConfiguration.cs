using ElectricalEmulator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ElectricalEmulator.Infra.Data.Persistence.Configurations
{
    public class InterlockConfiguration : IEntityTypeConfiguration<Interlock>
    {
        public void Configure(EntityTypeBuilder<Interlock> builder)
        {
            builder.Property(b => b.CreationDate)
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.InterlockGuid)
                .HasColumnType("UNIQUEIDENTIFIER ROWGUIDCOL")
                .HasDefaultValueSql("(newid())");

            builder.Property(b => b.RawValue)
                .IsRequired();
        }
    }
}
