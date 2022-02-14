using ElectricalEmulator.Domain.Entities;
using ElectricalEmulator.Domain.Enums;
using ElectricalEmulator.Domain.Interfaces;
using ElectricalEmulator.Infra.Data.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Infra.Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ElectricalEmulatorContext context)
            : base(context)
        {
        }

        public IQueryable<User> GetUser(string userId)
        {
            return _context.User
                .Where(u => u.Id.Equals(userId));
        }

        public IQueryable<User> GetUsers(IdentityRole identityRole = null, bool? isActive = null)
        {
            var users = _context.User.AsQueryable();

            if (identityRole != null)
            {
                users = _context.UserRoles.Where(ur => ur.RoleId.Equals(identityRole.Id))
                    .Join(users, ur => ur.UserId, u => u.Id, (ur, u) => u);
            }

            if (isActive != null)
            {
                users = users.Where(u => u.IsActive);
            }

            users = users.OrderByDescending(u => u.CreationDate);

            return users;
        }
    }
}
