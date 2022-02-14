using ElectricalEmulator.Application.ViewModels.PostElements;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ElectricalEmulator.Application.ViewModels.Posts
{
    public class PostRawValueViewModel
    {
        public PostRawValueViewModel()
        {
            PostElements = new List<PostElementViewModel>();
        }

        public string Name { get; set; }
        public List<PostElementViewModel> PostElements { get; set; }
    }
}
