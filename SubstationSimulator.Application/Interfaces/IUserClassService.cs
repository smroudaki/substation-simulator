using ElectricalEmulator.Application.Common.ViewModels;
using ElectricalEmulator.Application.ViewModels.UserClasses;
using ElectricalEmulator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Application.Interfaces
{
    public interface IUserClassService
    {
        Task<bool> IsExists(string userId, Guid classGuid);
        Task<Guid> CreateUserClass(UserClass userClass);
        Task<ResultViewModel> AcceptUserClass(Guid userClassGuid);
        Task<ResultViewModel> DeleteUserClass(Guid userClassGuid);
        Task<UserClassViewModel> GetUserClass(Guid userClassGuid);
        Task<List<UserClassViewModel>> GetUserClasses(string userId = null, Guid? classGuid = null, bool? isAccept = null);
        Task<int> GetUserClassesCount(string userId = null, Guid? classGuid = null, bool? isAccept = null);
    }
}
