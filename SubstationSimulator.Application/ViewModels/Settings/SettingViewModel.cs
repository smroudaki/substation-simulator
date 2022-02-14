using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ElectricalEmulator.Application.ViewModels.Settings
{
    public class SettingViewModel
    {
        public int SettingId { get; set; }
        public Guid SettingGuid { get; set; }
        [Display(Name = "امتیاز اولیه")]
        public int UserInitialScore { get; set; }
        [Display(Name = "زمان اولیه")]
        public int UserInitialRemainingTime { get; set; }
        [Display(Name = "هزینه ی هر ساعت شارژ")]
        public int UserPerHourChargeCost { get; set; }
    }
}
