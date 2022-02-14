using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricalEmulator.Domain.Entities
{
    public partial class Class
    {
        public Class()
        {
            Report = new HashSet<Report>();
            UserClass = new HashSet<UserClass>();
        }

        public int ClassId { get; set; }
        public Guid ClassGuid { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Report> Report { get; set; }
        public virtual ICollection<UserClass> UserClass { get; set; }
    }
}
