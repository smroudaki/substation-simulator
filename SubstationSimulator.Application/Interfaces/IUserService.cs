using ElectricalEmulator.Application.Common.ViewModels;
using ElectricalEmulator.Application.ViewModels.Accounts;
using ElectricalEmulator.Application.ViewModels.Users;
using ElectricalEmulator.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserViewModel> GetUser(string userId);
        Task<List<UserViewModel>> GetUsers(UserRole? userRole = null, bool? isActive = null);
        Task<int> GetUsersCount(UserRole? userRole = null, bool? isActive = null);
        Task<List<SelectListItem>> GetUsersPhoneNumberSelectListItem(UserRole? userRole = null, bool? isActive = null);
        Task<ResultViewModel> UpdateUser(string userId, EditProfileViewModel model);
        Task<int> DecreaseUserRemainingTime(string userId);
    }
}
