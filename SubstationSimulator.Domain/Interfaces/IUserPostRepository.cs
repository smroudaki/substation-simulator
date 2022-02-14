using ElectricalEmulator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectricalEmulator.Domain.Interfaces
{
    public interface IUserPostRepository : IRepository<UserPost>
    {
        IQueryable<UserPost> GetUserPost(string userId, Guid postGuid);
        IQueryable<UserPost> GetUserPosts(string userId = null, Guid? postGuid = null);
    }
}
