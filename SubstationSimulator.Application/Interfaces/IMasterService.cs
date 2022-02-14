using ElectricalEmulator.Application.Common.ViewModels;
using ElectricalEmulator.Application.ViewModels.UserClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Application.Interfaces
{
    public interface IMasterService
    {
        Task<List<UserClassViewModel>> GetRequests(string userId);
        Task<ResultViewModel> DeleteMaster(string userId);
    }
}
