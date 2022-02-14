using ElectricalEmulator.Application.Common;
using ElectricalEmulator.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricalEmulator.Application.ViewModels.Interlocks
{
    public class InterlockJsonResultViewModel
    {
        private int _locX, _locY;
        private float _rotZ;

        public string Name { get; set; }
        public ElementType TypeCode { get; set; }
        public int LocX
        {
            get { return _locX * Constants.elementsWidth; }
            set { _locX = value; }
        }
        public int LocY
        {
            get { return _locY * Constants.elementsHeight; }
            set { _locY = value; }
        }
        public float RotZ
        {
            get { return (float)Math.Round(_rotZ); }
            set { _rotZ = value; }
        }
        public bool Status { get; set; }
    }
}
