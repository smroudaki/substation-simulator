using ElectricalEmulator.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ElectricalEmulator.Infra.Data.Persistence
{
    public class ElectricalEmulatorContext : IdentityDbContext<User>
    {
        public ElectricalEmulatorContext(DbContextOptions<ElectricalEmulatorContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Class> Class { get; set; }
        public virtual DbSet<Interlock> Interlock { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<Report> Report { get; set; }
        public virtual DbSet<Setting> Setting { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserClass> UserClass { get; set; }
        public virtual DbSet<UserClassPost> UserClassPost { get; set; }
        public virtual DbSet<UserPost> UserPost { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Seed();
            base.OnModelCreating(builder);
        }
    }
}
