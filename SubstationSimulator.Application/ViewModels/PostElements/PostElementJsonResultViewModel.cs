using ElectricalEmulator.Application.Common;
using ElectricalEmulator.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ElectricalEmulator.Application.ViewModels.PostElements
{
    public class PostElementJsonResultViewModel
    {
        private int _locX, _locY;
        private float _rotZ;

        public string Name { get; set; }
        public ElementType TypeCode { get; set; }
        public Voltage? VoltageCode { get; set; }
        public Voltage? Ring1VoltageCode { get; set; }
        public Voltage? Ring2VoltageCode { get; set; }
        public Voltage? Ring3VoltageCode { get; set; }
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
        public Guid? InterlockGuid { get; set; }
        public Mode? Mode { get; set; }
        public SelectorSwitch? SelectorSwitch { get; set; }
        public WarningPanel? WarningPanel { get; set; }
        public int? Number { get; set; }
        public Bypass? Bypass { get; set; }
        public Service? Service { get; set; }
        public Recloser? Recloser { get; set; }
    }
}
