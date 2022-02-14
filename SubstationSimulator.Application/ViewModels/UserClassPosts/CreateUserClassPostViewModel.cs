using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ElectricalEmulator.Application.ViewModels.UserClassPosts
{
    public class CreateUserClassPostViewModel
    {
        [Required]
        public Guid UserClassGuid { get; set; }
        [Display(Name = "پست")]
        [Required(ErrorMessage = "لطفا پست را وارد کنید")]
        public Guid PostGuid { get; set; }
    }
}
