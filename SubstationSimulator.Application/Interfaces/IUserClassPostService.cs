using ElectricalEmulator.Application.Common.ViewModels;
using ElectricalEmulator.Application.ViewModels.UserClassPosts;
using ElectricalEmulator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Application.Interfaces
{
    public interface IUserClassPostService
    {
        Task<UserClassPostViewModel> GetUserClassPost(Guid userClassPostGuid, Guid postGuid);
        Task<ResultViewModel> CreateUserClassPost(UserClassPost userClassPost);
        Task<ResultViewModel> UpdateUserClassPost(Guid userClassPostGuid, Guid postGuid, object userClassPostChangesRaw);
    }
}
