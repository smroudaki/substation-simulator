using ElectricalEmulator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectricalEmulator.Domain.Interfaces
{
    public interface IUserClassPostRepository : IRepository<UserClassPost>
    {
        IQueryable<UserClassPost> GetUserClassPost(Guid userClassGuid, Guid postGuid);
        IQueryable<UserClassPost> GetUserClassPosts(Guid? userClassGuid = null, Guid? postGuid = null);
    }
}
