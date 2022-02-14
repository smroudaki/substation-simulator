using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ElectricalEmulator.Domain.Entities
{
    public partial class User : IdentityUser
    {
        public User()
        {
            Class = new HashSet<Class>();
            Report = new HashSet<Report>();
            UserClass = new HashSet<UserClass>();
            UserPost = new HashSet<UserPost>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? RemainingTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<Class> Class { get; set; }
        public virtual ICollection<Report> Report { get; set; }
        public virtual ICollection<UserClass> UserClass { get; set; }
        public virtual ICollection<UserPost> UserPost { get; set; }
    }
}
