using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ElectricalEmulator.Application.ViewModels.Users
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [Display(Name = "نام")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }
        [Display(Name = "شماره همراه")]
        public string PhoneNumber { get; set; }
        [Display(Name = "زمان باقی مانده")]
        public string RemainingTime { get; set; }
        [Display(Name = "وضعیت")]
        public string IsActive { get; set; }
        [Display(Name = "زمان ثبت")]
        public string CreationDate { get; set; }
        [Display(Name = "زمان آخرین ویرایش")]
        public string ModifiedDate { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + ' ' + LastName;
            }
        }
    }
}
