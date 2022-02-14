using ElectricalEmulator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectricalEmulator.Domain.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        IQueryable<Post> GetPost(Guid postGuid);
        IQueryable<Post> GetPosts(bool? isActive = null);
    }
}
