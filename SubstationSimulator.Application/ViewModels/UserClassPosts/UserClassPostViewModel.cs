using ElectricalEmulator.Application.ViewModels.Posts;
using ElectricalEmulator.Application.ViewModels.UserClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ElectricalEmulator.Application.ViewModels.UserClassPosts
{
    public class UserClassPostViewModel
    {
        public int UserClassPostId { get; set; }
        public Guid UserClassPostGuid { get; set; }
        public UserClassViewModel UserClass { get; set; }
        public PostViewModel Post { get; set; }
        [Display(Name = "تغییرات پست")]
        public string PostChanges { get; set; }
        [Display(Name = "زمان ثبت")]
        public string CreationDate { get; set; }
        [Display(Name = "زمان آخرین ویرایش")]
        public string ModifiedDate { get; set; }
    }
}
