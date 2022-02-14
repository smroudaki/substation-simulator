using ElectricalEmulator.Application.Common.ViewModels;
using ElectricalEmulator.Application.ViewModels.Classes;
using ElectricalEmulator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Application.Interfaces
{
    public interface IClassService
    {
        Task<ClassViewModel> GetClass(Guid classGuid);
        Task<List<ClassViewModel>> GetAvailableClasses(string userId);
        Task<List<ClassViewModel>> GetClasses(string userId = null, bool? isActive = null);
        Task<int> GetClassesCount(string userId = null, bool? isActive = null);
        Task<Guid> CreateClass(Class @class);
        Task<ResultViewModel> DeleteClass(Guid classGuid);
    }
}
