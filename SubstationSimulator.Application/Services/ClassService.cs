using ElectricalEmulator.Application.Common;
using ElectricalEmulator.Application.Common.Extensions;
using ElectricalEmulator.Application.Common.ViewModels;
using ElectricalEmulator.Application.Interfaces;
using ElectricalEmulator.Application.ViewModels.Classes;
using ElectricalEmulator.Application.ViewModels.Users;
using ElectricalEmulator.Domain.Entities;
using ElectricalEmulator.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Application.Services
{
    public class ClassService : IClassService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClassService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreateClass(Class @class)
        {
            _unitOfWork.Classes.Insert(@class);
            await _unitOfWork.CommitAsync();
            return @class.ClassGuid;
        }

        public async Task<ResultViewModel> DeleteClass(Guid classGuid)
        {
            var @class = await _unitOfWork.Classes
                .GetClass(classGuid)
                .SingleOrDefaultAsync();

            if (@class == null)
            {
                return ResultViewModel.Failure();
            }

            var userClasses = await _unitOfWork.UserClasses
                .GetUserClasses(null, @class.ClassGuid)
                .ToListAsync();

            foreach (var userClass in userClasses)
            {
                var userClassPosts = await _unitOfWork.UserClassPosts
                    .GetUserClassPosts(userClass.UserClassGuid)
                    .ToListAsync();

                foreach (var userClassPost in userClassPosts)
                {
                    _unitOfWork.UserClassPosts.Delete(userClassPost);
                }

                _unitOfWork.UserClasses.Delete(userClass);
            }

            _unitOfWork.Classes.Delete(@class);

            await _unitOfWork.CommitAsync();

            return ResultViewModel.Success();
        }

        public async Task<List<ClassViewModel>> GetAvailableClasses(string userId)
        {
            var userClasses = await _unitOfWork.UserClasses
                .GetUserClasses(userId)
                .Select(uc => uc.ClassId)
                .ToListAsync();

            return await _unitOfWork.Classes.GetClasses(null, true, userClasses)
                .Select(c => new ClassViewModel
                {
                    ClassId = c.ClassId,
                    ClassGuid = c.ClassGuid,
                    User = new UserViewModel
                    {
                        Id = c.User.Id,
                        FirstName = c.User.FirstName,
                        LastName = c.User.LastName,
                        PhoneNumber = c.User.PhoneNumber,
                        RemainingTime = c.User.RemainingTime.HasValue ? c.User.RemainingTime.Value.ToString() : Constants.NotSet,
                        IsActive = c.User.IsActive ? Constants.Active : Constants.InActive,
                        CreationDate = PersianDateExtensions.ToPeString(c.User.CreationDate, "yyyy/MM/dd HH:mm"),
                        ModifiedDate = c.User.ModifiedDate == null ?
                            Constants.NotSet : PersianDateExtensions.ToPeString(c.User.ModifiedDate.Value, "yyyy/MM/dd HH:mm")

                    },
                    Title = c.Title,
                    Description = c.Description,
                    IsActive = c.IsActive ? Constants.Active : Constants.InActive,
                    CreationDate = PersianDateExtensions.ToPeString(c.CreationDate, "yyyy/MM/dd HH:mm"),
                    ModifiedDate = c.ModifiedDate == null ?
                        Constants.NotSet : PersianDateExtensions.ToPeString(c.ModifiedDate.Value, "yyyy/MM/dd HH:mm")

                }).ToListAsync();
        }

        public async Task<ClassViewModel> GetClass(Guid classGuid)
        {
            return await _unitOfWork.Classes.GetClass(classGuid)
                .Select(c => new ClassViewModel
                {
                    ClassId = c.ClassId,
                    ClassGuid = c.ClassGuid,
                    User = new UserViewModel
                    {
                        Id = c.User.Id,
                        FirstName = c.User.FirstName,
                        LastName = c.User.LastName,
                        PhoneNumber = c.User.PhoneNumber,
                        RemainingTime = c.User.RemainingTime.HasValue ? c.User.RemainingTime.Value.ToString() : Constants.NotSet,
                        IsActive = c.User.IsActive ? Constants.Active : Constants.InActive,
                        CreationDate = PersianDateExtensions.ToPeString(c.User.CreationDate, "yyyy/MM/dd HH:mm"),
                        ModifiedDate = c.User.ModifiedDate == null ?
                            Constants.NotSet : PersianDateExtensions.ToPeString(c.User.ModifiedDate.Value, "yyyy/MM/dd HH:mm")

                    },
                    Title = c.Title,
                    Description = c.Description,
                    IsActive = c.IsActive ? Constants.Active : Constants.InActive,
                    CreationDate = PersianDateExtensions.ToPeString(c.CreationDate, "yyyy/MM/dd HH:mm"),
                    ModifiedDate = c.ModifiedDate == null ?
                        Constants.NotSet : PersianDateExtensions.ToPeString(c.ModifiedDate.Value, "yyyy/MM/dd HH:mm")

                }).SingleOrDefaultAsync();
        }

        public async Task<List<ClassViewModel>> GetClasses(string userId = null, bool? isActive = null)
        {
            return await _unitOfWork.Classes.GetClasses(userId, isActive)
                .Select(c => new ClassViewModel
                {
                    ClassId = c.ClassId,
                    ClassGuid = c.ClassGuid,
                    User = new UserViewModel
                    {
                        Id = c.User.Id,
                        FirstName = c.User.FirstName,
                        LastName = c.User.LastName,
                        PhoneNumber = c.User.PhoneNumber,
                        RemainingTime = c.User.RemainingTime.HasValue ? c.User.RemainingTime.Value.ToString() : Constants.NotSet,
                        IsActive = c.User.IsActive ? Constants.Active : Constants.InActive,
                        CreationDate = PersianDateExtensions.ToPeString(c.User.CreationDate, "yyyy/MM/dd HH:mm"),
                        ModifiedDate = c.User.ModifiedDate == null ?
                            Constants.NotSet : PersianDateExtensions.ToPeString(c.User.ModifiedDate.Value, "yyyy/MM/dd HH:mm")

                    },
                    Title = c.Title,
                    Description = c.Description,
                    IsActive = c.IsActive ? Constants.Active : Constants.InActive,
                    CreationDate = PersianDateExtensions.ToPeString(c.CreationDate, "yyyy/MM/dd HH:mm"),
                    ModifiedDate = c.ModifiedDate == null ?
                        Constants.NotSet : PersianDateExtensions.ToPeString(c.ModifiedDate.Value, "yyyy/MM/dd HH:mm")

                }).ToListAsync();
        }

        public async Task<int> GetClassesCount(string userId = null, bool? isActive = null)
        {
            return await _unitOfWork.Classes
                .GetClasses(userId, isActive)
                .CountAsync();
        }
    }
}
