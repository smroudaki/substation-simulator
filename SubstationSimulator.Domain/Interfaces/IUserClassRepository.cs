using ElectricalEmulator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Domain.Interfaces
{
    public interface IUserClassRepository : IRepository<UserClass>
    {
        IQueryable<UserClass> GetUserClass(Guid userClassGuid);
        IQueryable<UserClass> GetUserClass(string userId, Guid classGuid);
        IQueryable<UserClass> GetUserClasses(string userId = null, Guid? classGuid = null, bool? isAccept = null);
        IQueryable<UserClass> GetUserClasses(IQueryable<Class> classes, string userId = null, bool? isAccept = null);
    }
}
