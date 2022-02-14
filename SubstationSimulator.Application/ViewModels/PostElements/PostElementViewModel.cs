using ElectricalEmulator.Application.ViewModels.Interlocks;
using ElectricalEmulator.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricalEmulator.Application.ViewModels.PostElements
{
    public class PostElementViewModel
    {
        public PostElementViewModel()
        {
            Interlocks = new List<InterlockViewModel>();
        }

        public string Name { get; set; }
        public ElementType TypeCode { get; set; }
        public Voltage VoltageCode { get; set; }
        public int LocX { get; set; }
        public int LocY { get; set; }
        public float RotZ { get; set; }
        public int Status { get; set; }
        public List<InterlockViewModel> Interlocks { get; set; }
        public Mode Mode { get; set; }
        public SelectorSwitch SelectorSwitch { get; set; }
        public WarningPanel WarningPanel { get; set; }
        public int Number { get; set; }
        public Bypass Bypass { get; set; }
        public Service Service { get; set; }
        public Recloser Recloser { get; set; }
    }
}
