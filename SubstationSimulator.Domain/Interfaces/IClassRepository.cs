using ElectricalEmulator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectricalEmulator.Domain.Interfaces
{
    public interface IClassRepository : IRepository<Class>
    {
        IQueryable<Class> GetClass(Guid classGuid);
        IQueryable<Class> GetClasses(string userId = null, bool? isActive = null, List<int> classIdExceptions = null);
    }
}
