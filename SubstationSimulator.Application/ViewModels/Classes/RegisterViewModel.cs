using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ElectricalEmulator.Application.ViewModels.Classes
{
    public class RegisterViewModel
    {
        [Display(Name = "دانشجو")]
        [Required(ErrorMessage = "لطفا دانشجو را وارد کنید")]
        public string UserId { get; set; }
        public Guid ClassGuid { get; set; }
    }
}
