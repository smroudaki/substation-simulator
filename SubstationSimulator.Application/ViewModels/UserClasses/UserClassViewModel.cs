using ElectricalEmulator.Application.ViewModels.Classes;
using ElectricalEmulator.Application.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ElectricalEmulator.Application.ViewModels.UserClasses
{
    public class UserClassViewModel
    {
        public int UserClassId { get; set; }
        public Guid UserClassGuid { get; set; }
        [Display(Name = "دانشجو")]
        public UserViewModel User { get; set; }
        public ClassViewModel Class { get; set; }
        [Display(Name = "وضعیت")]
        public string IsAccept { get; set; }
        [Display(Name = "زمان پذیرش")]
        public string AcceptanceDate { get; set; }
        [Display(Name = "زمان ثبت")]
        public string CreationDate { get; set; }
    }
}
