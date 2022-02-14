using ElectricalEmulator.Domain.Entities;
using ElectricalEmulator.Domain.Interfaces;
using ElectricalEmulator.Infra.Data.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectricalEmulator.Infra.Data.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(ElectricalEmulatorContext context)
            : base(context)
        {
        }

        public IQueryable<Post> GetPost(Guid postGuid)
        {
            return _context.Post
                .Where(p => p.PostGuid.Equals(postGuid));
        }

        public IQueryable<Post> GetPosts(bool? isActive = null)
        {
            var post = _context.Post.AsQueryable();

            if (isActive != null)
            {
                post = post.Where(p => p.IsActive.Equals(isActive));
            }

            return post.OrderByDescending(c => c.CreationDate);
        }
    }
}
