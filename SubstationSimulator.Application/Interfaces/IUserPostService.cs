using ElectricalEmulator.Application.Common.ViewModels;
using ElectricalEmulator.Application.ViewModels.UserPosts;
using ElectricalEmulator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Application.Interfaces
{
    public interface IUserPostService
    {
        Task<UserPostViewModel> GetUserPost(string userId, Guid postGuid);
        Task<ResultViewModel> CreateUserPost(UserPost userPost);
        Task<ResultViewModel> UpdateUserPost(string userId, Guid postGuid, object userPostChangesRaw);
    }
}
