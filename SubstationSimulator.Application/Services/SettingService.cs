using ElectricalEmulator.Application.Interfaces;
using ElectricalEmulator.Application.ViewModels.Settings;
using ElectricalEmulator.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Application.Services
{
    public class SettingService : ISettingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SettingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SettingViewModel> GetSettings()
        {
            return await _unitOfWork.Settings
                .GetSettings()
                .Select(s => new SettingViewModel
                {
                    SettingId = s.SettingId,
                    SettingGuid = s.SettingGuid,
                    UserInitialScore = s.UserInitialScore,
                    UserInitialRemainingTime = s.UserInitialRemainingTime,
                    UserPerHourChargeCost = s.UserPerHourChargeCost

                }).FirstOrDefaultAsync();
        }
    }
}
