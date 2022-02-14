using ElectricalEmulator.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Domain.Interfaces
{
    public interface IRoleRepository : IRepository<IdentityRole>
    {
        IQueryable<IdentityRole> GetRole(UserRole role);
    }
}
