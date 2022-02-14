using ElectricalEmulator.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        IQueryable<User> GetUser(string userId);
        IQueryable<User> GetUsers(IdentityRole identityRole = null, bool? isActive = null);
    }
}
