using ElectricalEmulator.Application.ViewModels.PostElements;
using ElectricalEmulator.Application.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ElectricalEmulator.Application.ViewModels.Posts
{
    public class PostViewModel
    {
        public int PostId { get; set; }
        public Guid PostGuid { get; set; }
        public string RawValue { get; set; }
        public PostRawValueViewModel RawValueDeserialized { get; set; }
        public string IsActive { get; set; }
        public string CreationDate { get; set; }
        public string ModifiedDate { get; set; }
    }
}
