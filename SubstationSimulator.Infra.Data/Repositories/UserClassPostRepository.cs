using ElectricalEmulator.Domain.Entities;
using ElectricalEmulator.Domain.Interfaces;
using ElectricalEmulator.Infra.Data.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectricalEmulator.Infra.Data.Repositories
{
    public class UserClassPostRepository : BaseRepository<UserClassPost>, IUserClassPostRepository
    {
        public UserClassPostRepository(ElectricalEmulatorContext context)
            : base(context)
        {
        }

        public IQueryable<UserClassPost> GetUserClassPost(Guid userClassGuid, Guid postGuid)
        {
            return _context.UserClassPost
                .Where(ucp => ucp.UserClass.UserClassGuid.Equals(userClassGuid) && ucp.Post.PostGuid.Equals(postGuid));
        }

        public IQueryable<UserClassPost> GetUserClassPosts(Guid? userClassGuid = null, Guid? postGuid = null)
        {
            var userClassPost = _context.UserClassPost.AsQueryable();

            if (userClassGuid != null)
            {
                userClassPost = userClassPost.Where(ucp => ucp.UserClass.UserClassGuid.Equals(userClassGuid));
            }

            if (postGuid != null)
            {
                userClassPost = userClassPost.Where(ucp => ucp.Post.PostGuid.Equals(postGuid));
            }

            return userClassPost.OrderByDescending(ucp => ucp.CreationDate);
        }
    }
}
