using ElectricalEmulator.Application.ViewModels.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Application.Interfaces
{
    public interface ISettingService
    {
        Task<SettingViewModel> GetSettings();
    }
}
