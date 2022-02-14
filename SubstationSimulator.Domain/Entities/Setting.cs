using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricalEmulator.Domain.Entities
{
    public partial class Setting
    {
        public int SettingId { get; set; }
        public Guid SettingGuid { get; set; }
        public int UserInitialScore { get; set; }
        public int UserInitialRemainingTime { get; set; }
        public int UserPerHourChargeCost { get; set; }
    }
}
