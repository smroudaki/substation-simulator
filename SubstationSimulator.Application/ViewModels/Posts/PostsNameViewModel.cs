using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricalEmulator.Application.ViewModels.Posts
{
    public class PostsNameViewModel
    {
        public PostsNameViewModel()
        {
            Posts = new List<PostNameViewModel>();
        }

        public List<PostNameViewModel> Posts { get; set; }
    }
}
