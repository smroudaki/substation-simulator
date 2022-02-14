using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ElectricalEmulator.Application.ViewModels.UserPosts
{
    public class CreateUserPostViewModel
    {
        [Required]
        public Guid PostGuid { get; set; }
    }
}
