using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ElectricalEmulator.Application.ViewModels.Accounts
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "کلمه عبور فعلی")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "لطفا رمز فعلی را وارد نمایید")]
        public string CurrentPassword { get; set; }

        [Display(Name = "کلمه عبور جدید")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "لطفا رمز جدید را وارد نمایید")]
        public string NewPassword { get; set; }

        [Display(Name = "تکرار کلمه عبور جدید")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "کلمه عبور جدید و تایید کلمه عبور جدید مطابق نیست")]
        [Required(ErrorMessage = "لطفا تکرار کلمه عبور جدید را وارد نمایید")]
        public string ConfirmNewPassword { get; set; }
    }
}
