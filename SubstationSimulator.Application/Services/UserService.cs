using ElectricalEmulator.Application.Common;
using ElectricalEmulator.Application.Common.Extensions;
using ElectricalEmulator.Application.Common.ViewModels;
using ElectricalEmulator.Application.Interfaces;
using ElectricalEmulator.Application.ViewModels.Accounts;
using ElectricalEmulator.Application.ViewModels.Users;
using ElectricalEmulator.Domain.Enums;
using ElectricalEmulator.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> DecreaseUserRemainingTime(string userId)
        {
            var user = await _unitOfWork.Users
                .GetUser(userId)
                .SingleOrDefaultAsync();

            if (user == null)
            {
                return -1;
            }

            if (!user.RemainingTime.HasValue)
            {
                return -2;
            }

            if (user.RemainingTime.Value <= 0)
            {
                return 0;
            }

            user.RemainingTime -= Constants.DecreasingRemainigTimeAmount;
            user.ModifiedDate = DateTime.Now;

            await _unitOfWork.CommitAsync();

            return user.RemainingTime.Value;
        }

        public async Task<UserViewModel> GetUser(string userId)
        {
            return await _unitOfWork.Users
                .GetUser(userId)
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    PhoneNumber = u.PhoneNumber,
                    RemainingTime = u.RemainingTime.HasValue ? u.RemainingTime.Value.ToString() : Constants.NotSet,
                    IsActive = u.IsActive ? Constants.Active : Constants.InActive,
                    CreationDate = PersianDateExtensions.ToPeString(u.CreationDate, "yyyy/MM/dd HH:mm"),
                    ModifiedDate = u.ModifiedDate == null ?
                        Constants.NotSet : PersianDateExtensions.ToPeString(u.ModifiedDate.Value, "yyyy/MM/dd HH:mm")

                }).SingleOrDefaultAsync();
        }

        public async Task<List<UserViewModel>> GetUsers(UserRole? userRole = null, bool? isActive = null)
        {
            IdentityRole identityRole = null;

            if (userRole != null)
            {
                identityRole = await _unitOfWork.Roles.GetRole(userRole.Value)
                    .SingleOrDefaultAsync();

                if (identityRole == null)
                {
                    return new List<UserViewModel>();
                }
            }

            var users = _unitOfWork.Users.GetUsers(identityRole, isActive);

            return await users.Select(u => new UserViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                RemainingTime = u.RemainingTime.HasValue ? u.RemainingTime.Value.ToString() : Constants.NotSet,
                IsActive = u.IsActive ? Constants.Active : Constants.InActive,
                CreationDate = PersianDateExtensions.ToPeString(u.CreationDate, "yyyy/MM/dd HH:mm"),
                ModifiedDate = u.ModifiedDate == null ?
                    Constants.NotSet : PersianDateExtensions.ToPeString(u.ModifiedDate.Value, "yyyy/MM/dd HH:mm")

            }).ToListAsync();
        }

        public async Task<int> GetUsersCount(UserRole? userRole = null, bool? isActive = null)
        {
            IdentityRole identityRole = null;

            if (userRole != null)
            {
                identityRole = await _unitOfWork.Roles.GetRole(userRole.Value)
                    .SingleOrDefaultAsync();

                if (identityRole == null)
                {
                    return 0;
                }
            }

            return await _unitOfWork.Users
                .GetUsers(identityRole, isActive)
                .CountAsync();
        }

        public async Task<List<SelectListItem>> GetUsersPhoneNumberSelectListItem(UserRole? userRole = null, bool? isActive = null)
        {
            IdentityRole identityRole = null;

            if (userRole != null)
            {
                identityRole = await _unitOfWork.Roles.GetRole(userRole.Value)
                    .SingleOrDefaultAsync();

                if (identityRole == null)
                {
                    return new List<SelectListItem>();
                }
            }

            var users = _unitOfWork.Users.GetUsers(identityRole, isActive);

            return await users.Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = u.PhoneNumber

            }).ToListAsync();
        }

        public async Task<ResultViewModel> UpdateUser(string userId, EditProfileViewModel model)
        {
            var user = await _unitOfWork.Users
                .GetUser(userId)
                .SingleOrDefaultAsync();

            if (user == null)
            {
                return ResultViewModel.Failure();
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.ModifiedDate = DateTime.Now;

            await _unitOfWork.CommitAsync();

            return ResultViewModel.Success();
        }
    }
}
