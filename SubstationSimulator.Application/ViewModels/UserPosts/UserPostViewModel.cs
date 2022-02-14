using ElectricalEmulator.Application.ViewModels.Posts;
using ElectricalEmulator.Application.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ElectricalEmulator.Application.ViewModels.UserPosts
{
    public class UserPostViewModel
    {
        public int UserPostId { get; set; }
        public Guid UserPostGuid { get; set; }
        public UserViewModel User { get; set; }
        public PostViewModel Post { get; set; }
        [Display(Name = "تغییرات پست")]
        public string PostChanges { get; set; }
        [Display(Name = "زمان ثبت")]
        public string CreationDate { get; set; }
        [Display(Name = "زمان آخرین ویرایش")]
        public string ModifiedDate { get; set; }
    }
}
