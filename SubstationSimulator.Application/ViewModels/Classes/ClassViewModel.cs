using ElectricalEmulator.Application.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace ElectricalEmulator.Application.ViewModels.Classes
{
    public class ClassViewModel
    {
        public int ClassId { get; set; }
        public Guid ClassGuid { get; set; }
        [Display(Name = "استاد")]
        public UserViewModel User { get; set; }
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
        [Display(Name = "وضعیت")]
        public string IsActive { get; set; }
        [Display(Name = "زمان ثبت")]
        public string CreationDate { get; set; }
        [Display(Name = "زمان آخرین ویرایش")]
        public string ModifiedDate { get; set; }
    }
}
