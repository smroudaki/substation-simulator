using System;
using System.Collections.Generic;

namespace ElectricalEmulator.Domain.Entities
{
    public partial class Post
    {
        public Post()
        {
            Report = new HashSet<Report>();
            UserClassPost = new HashSet<UserClassPost>();
            UserPost = new HashSet<UserPost>();
        }

        public int PostId { get; set; }
        public Guid PostGuid { get; set; }
        public string RawValue { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<Report> Report { get; set; }
        public virtual ICollection<UserClassPost> UserClassPost { get; set; }
        public virtual ICollection<UserPost> UserPost { get; set; }
    }
}
