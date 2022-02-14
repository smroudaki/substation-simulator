using ElectricalEmulator.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricalEmulator.Infra.Data.Persistence
{
    public static class ElectricalEmulatorContextSeed
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            #region IdentityRole

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                },
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Master",
                    NormalizedName = "Master".ToUpper()
                },
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Student",
                    NormalizedName = "Student".ToUpper()
                }
            );

            #endregion

            #region Setting

            modelBuilder.Entity<Setting>().HasData(
                new Setting
                {
                    SettingId = 1,
                    UserInitialScore = 100,
                    UserInitialRemainingTime = 3600,
                    UserPerHourChargeCost = 2000
                }
            );

            #endregion
        }
    }
}
