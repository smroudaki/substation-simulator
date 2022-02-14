using ElectricalEmulator.Application.ViewModels.PostElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricalEmulator.Application.ViewModels.Posts
{
    public class PostJsonResultViewModel
    {
        public PostJsonResultViewModel()
        {
            PostElements = new List<PostElementJsonResultViewModel>();
        }

        public string Name { get; set; }
        public List<PostElementJsonResultViewModel> PostElements { get; set; }
    }
}
