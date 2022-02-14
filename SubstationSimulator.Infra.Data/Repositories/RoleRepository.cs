using ElectricalEmulator.Domain.Enums;
using ElectricalEmulator.Domain.Interfaces;
using ElectricalEmulator.Infra.Data.Persistence;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Infra.Data.Repositories
{
    public class RoleRepository : BaseRepository<IdentityRole>, IRoleRepository
    {
        public RoleRepository(ElectricalEmulatorContext context)
            : base(context)
        {
        }

        public IQueryable<IdentityRole> GetRole(UserRole role)
        {
            return _context.Roles
                .Where(r => r.Name.Equals(role.ToString()));
        }
    }
}
