using ElectricalEmulator.Application.ViewModels.Classes;
using ElectricalEmulator.Application.ViewModels.Posts;
using ElectricalEmulator.Application.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ElectricalEmulator.Application.ViewModels.Reports
{
    public class ReportViewModel
    {
        public int ReportId { get; set; }
        public Guid ReportGuid { get; set; }
        [Display(Name = "کاربر")]
        public UserViewModel User { get; set; }
        public PostViewModel Post { get; set; }
        public ClassViewModel Class { get; set; }
        [Display(Name = "مقدار")]
        public string Value { get; set; }
        [Display(Name = "زمان ثبت")]
        public string CreationDate { get; set; }
    }
}
