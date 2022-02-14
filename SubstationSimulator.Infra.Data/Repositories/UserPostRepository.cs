using ElectricalEmulator.Domain.Entities;
using ElectricalEmulator.Domain.Interfaces;
using ElectricalEmulator.Infra.Data.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectricalEmulator.Infra.Data.Repositories
{
    public class UserPostRepository : BaseRepository<UserPost>, IUserPostRepository
    {
        public UserPostRepository(ElectricalEmulatorContext context)
            : base(context)
        {
        }

        public IQueryable<UserPost> GetUserPost(string userId, Guid postGuid)
        {
            return _context.UserPost
                .Where(up => up.UserId.Equals(userId) && up.Post.PostGuid.Equals(postGuid));
        }

        public IQueryable<UserPost> GetUserPosts(string userId = null, Guid? postGuid = null)
        {
            var userPost = _context.UserPost.AsQueryable();

            if (!string.IsNullOrEmpty(userId))
            {
                userPost = userPost.Where(up => up.UserId.Equals(userId));
            }

            if (postGuid != null)
            {
                userPost = userPost.Where(up => up.Post.PostGuid.Equals(postGuid));
            }

            return userPost.OrderByDescending(up => up.CreationDate);
        }
    }
}
