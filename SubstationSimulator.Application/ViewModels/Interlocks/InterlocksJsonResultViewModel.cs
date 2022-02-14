using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricalEmulator.Application.ViewModels.Interlocks
{
    public class InterlocksJsonResultViewModel
    {
        public InterlocksJsonResultViewModel()
        {
            Interlocks = new List<InterlockJsonResultViewModel>();
        }

        public List<InterlockJsonResultViewModel> Interlocks { get; set; }
    }
}
