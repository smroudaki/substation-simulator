using ElectricalEmulator.Application.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Application.Interfaces
{
    public interface IStudentService
    {
        Task<ResultViewModel> DeleteStudent(string userId);
    }
}
