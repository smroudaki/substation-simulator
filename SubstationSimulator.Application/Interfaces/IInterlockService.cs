using ElectricalEmulator.Application.ViewModels.Interlocks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Application.Interfaces
{
    public interface IInterlockService
    {
        Task<InterlockViewModel> GetInterlock(Guid interlockGuid);
    }
}
