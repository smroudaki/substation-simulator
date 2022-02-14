using ElectricalEmulator.Domain.Entities;
using ElectricalEmulator.Domain.Interfaces;
using ElectricalEmulator.Infra.Data.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectricalEmulator.Infra.Data.Repositories
{
    public class ClassRepository : BaseRepository<Class>, IClassRepository
    {
        public ClassRepository(ElectricalEmulatorContext context)
            : base(context)
        {
        }

        public IQueryable<Class> GetClass(Guid classGuid)
        {
            return Get(c => c.ClassGuid.Equals(classGuid), null, "User");
        }

        public IQueryable<Class> GetClasses(string userId = null, bool? isActive = null, List<int> classIdExceptions = null)
        {
            var @class = _context.Class.AsQueryable();

            if (!string.IsNullOrEmpty(userId))
            {
                @class = @class.Where(c => c.UserId.Equals(userId));
            }

            if (isActive != null)
            {
                @class = @class.Where(c => c.IsActive.Equals(isActive));
            }

            if (classIdExceptions != null)
            {
                @class = @class.Where(c => !classIdExceptions.Contains(c.ClassId));
            }

            return @class.OrderByDescending(c => c.CreationDate);
        }
    }
}
