using ElectricalEmulator.Domain.Entities;
using ElectricalEmulator.Domain.Interfaces;
using ElectricalEmulator.Infra.Data.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Infra.Data.Repositories
{
    public class UserClassRepository : BaseRepository<UserClass>, IUserClassRepository
    {
        public UserClassRepository(ElectricalEmulatorContext context)
            : base(context)
        {
        }

        public IQueryable<UserClass> GetUserClass(Guid userClassGuid)
        {
            return _context.UserClass
                .Where(uc => uc.UserClassGuid.Equals(userClassGuid));
        }

        public IQueryable<UserClass> GetUserClass(string userId, Guid classGuid)
        {
            return _context.UserClass
                .Where(uc => uc.UserId.Equals(userId) && uc.Class.ClassGuid.Equals(classGuid));
        }

        public IQueryable<UserClass> GetUserClasses(string userId = null, Guid? classGuid = null, bool? isAccept = null)
        {
            var userClass = _context.UserClass.AsQueryable();

            if (!string.IsNullOrEmpty(userId))
            {
                userClass = userClass.Where(uc => uc.UserId.Equals(userId));
            }

            if (classGuid != null)
            {
                userClass = userClass.Where(uc => uc.Class.ClassGuid.Equals(classGuid));
            }

            if (isAccept != null)
            {
                userClass = userClass.Where(uc => uc.IsAccept.Equals(isAccept));
            }

            return userClass.OrderByDescending(uc => uc.CreationDate);
        }

        public IQueryable<UserClass> GetUserClasses(IQueryable<Class> classes, string userId = null, bool? isAccept = null)
        {
            var userClass = _context.UserClass.AsQueryable();

            if (!string.IsNullOrEmpty(userId))
            {
                userClass = userClass.Where(uc => uc.UserId.Equals(userId));
            }

            if (isAccept != null)
            {
                userClass = userClass.Where(uc => uc.IsAccept.Equals(isAccept));
            }

            return userClass.Join(classes, uc => uc.ClassId, c => c.ClassId, (uc, c) => uc)
                .OrderByDescending(uc => uc.CreationDate);
        }
    }
}
