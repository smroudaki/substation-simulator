using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricalEmulator.Domain.Entities
{
    public partial class UserClassPost
    {
        public int UserClassPostId { get; set; }
        public Guid UserClassPostGuid { get; set; }
        public int UserClassId { get; set; }
        public int PostId { get; set; }
        public string PostChanges { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Post Post { get; set; }
        public virtual UserClass UserClass { get; set; }
    }
}
