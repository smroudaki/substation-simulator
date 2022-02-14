using ElectricalEmulator.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricalEmulator.Application.ViewModels.Interlocks
{
    public class InterlockRawValueViewModel
    {
        public string Name { get; set; }
        public ElementType TypeCode { get; set; }
        public int LocX { get; set; }
        public int LocY { get; set; }
        public float RotZ { get; set; }
    }
}
