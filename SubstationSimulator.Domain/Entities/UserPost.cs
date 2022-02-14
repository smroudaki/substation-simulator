using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricalEmulator.Domain.Entities
{
    public partial class UserPost
    {
        public int UserPostId { get; set; }
        public Guid UserPostGuid { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        public string PostChanges { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
