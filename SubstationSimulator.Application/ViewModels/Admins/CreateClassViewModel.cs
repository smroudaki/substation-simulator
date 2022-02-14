using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ElectricalEmulator.Application.ViewModels.Admins
{
    public class CreateClassViewModel
    {
        [Display(Name = "استاد")]
        [Required(ErrorMessage = "لطفا استاد را وارد کنید")]
        public string UserId { get; set; }
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا عنوان را وارد کنید")]
        public string Title { get; set; }
        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا توضیحات را وارد کنید")]
        public string Description { get; set; }
    }
}
