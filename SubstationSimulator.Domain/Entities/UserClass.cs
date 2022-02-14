using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricalEmulator.Domain.Entities
{
    public partial class UserClass
    {
        public UserClass()
        {
            UserClassPost = new HashSet<UserClassPost>();
        }

        public int UserClassId { get; set; }
        public Guid UserClassGuid { get; set; }
        public string UserId { get; set; }
        public int ClassId { get; set; }
        public bool IsAccept { get; set; }
        public DateTime? AcceptanceDate { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual Class Class { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<UserClassPost> UserClassPost { get; set; }
    }
}
