using ElectricalEmulator.Application.Common;
using ElectricalEmulator.Application.Common.Extensions;
using ElectricalEmulator.Application.Common.ViewModels;
using ElectricalEmulator.Application.Interfaces;
using ElectricalEmulator.Application.ViewModels.Classes;
using ElectricalEmulator.Application.ViewModels.UserClasses;
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
    public class UserClassService : IUserClassService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserClassService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultViewModel> AcceptUserClass(Guid userClassGuid)
        {
            var userClass = await _unitOfWork.UserClasses
                .GetUserClass(userClassGuid)
                .SingleOrDefaultAsync();

            if (userClass == null)
            {
                return ResultViewModel.Failure();
            }

            userClass.IsAccept = true;
            userClass.AcceptanceDate = DateTime.Now;

            await _unitOfWork.CommitAsync();

            return ResultViewModel.Success();
        }

        public async Task<Guid> CreateUserClass(UserClass userClass)
        {
            _unitOfWork.UserClasses.Insert(userClass);
            await _unitOfWork.CommitAsync();
            return userClass.UserClassGuid;
        }

        public async Task<ResultViewModel> DeleteUserClass(Guid userClassGuid)
        {
            var userClass = await _unitOfWork.UserClasses
                .GetUserClass(userClassGuid)
                .SingleOrDefaultAsync();

            if (userClass == null)
            {
                return ResultViewModel.Failure();
            }

            var userClassPosts = await _unitOfWork.UserClassPosts
                .GetUserClassPosts(userClass.UserClassGuid)
                .ToListAsync();

            foreach (var userClassPost in userClassPosts)
            {
                _unitOfWork.UserClassPosts.Delete(userClassPost);
            }

            _unitOfWork.UserClasses.Delete(userClass);

            await _unitOfWork.CommitAsync();

            return ResultViewModel.Success();
        }

        public async Task<UserClassViewModel> GetUserClass(Guid userClassGuid)
        {
            return await _unitOfWork.UserClasses
                .GetUserClass(userClassGuid)
                .Select(uc => new UserClassViewModel
                {
                    UserClassId = uc.UserClassId,
                    UserClassGuid = uc.UserClassGuid,
                    User = new UserViewModel
                    {
                        Id = uc.User.Id,
                        FirstName = uc.User.FirstName,
                        LastName = uc.User.LastName,
                        PhoneNumber = uc.User.PhoneNumber,
                        RemainingTime = uc.User.RemainingTime.HasValue ? uc.User.RemainingTime.Value.ToString() : Constants.NotSet,
                        IsActive = uc.User.IsActive ? Constants.Active : Constants.InActive,
                        CreationDate = PersianDateExtensions.ToPeString(uc.CreationDate, "yyyy/MM/dd HH:mm"),
                        ModifiedDate = uc.User.ModifiedDate == null ?
                            Constants.NotSet : PersianDateExtensions.ToPeString(uc.User.ModifiedDate.Value, "yyyy/MM/dd HH:mm")
                    },
                    Class = new ClassViewModel
                    {
                        ClassId = uc.Class.ClassId,
                        ClassGuid = uc.Class.ClassGuid,
                        User = new UserViewModel
                        {
                            Id = uc.Class.User.Id,
                            FirstName = uc.Class.User.FirstName,
                            LastName = uc.Class.User.LastName,
                            PhoneNumber = uc.Class.User.PhoneNumber,
                            RemainingTime = uc.Class.User.RemainingTime.HasValue ? uc.User.RemainingTime.Value.ToString() : Constants.NotSet,
                            IsActive = uc.Class.User.IsActive ? Constants.Active : Constants.InActive,
                            CreationDate = PersianDateExtensions.ToPeString(uc.Class.User.CreationDate, "yyyy/MM/dd HH:mm"),
                            ModifiedDate = uc.Class.User.ModifiedDate == null ?
                                Constants.NotSet : PersianDateExtensions.ToPeString(uc.Class.User.ModifiedDate.Value, "yyyy/MM/dd HH:mm")
                        },
                        Title = uc.Class.Title,
                        Description = uc.Class.Description,
                        CreationDate = PersianDateExtensions.ToPeString(uc.Class.CreationDate, "yyyy/MM/dd HH:mm"),
                        ModifiedDate = uc.Class.ModifiedDate == null ?
                            Constants.NotSet : PersianDateExtensions.ToPeString(uc.Class.ModifiedDate.Value, "yyyy/MM/dd HH:mm")
                    },
                    IsAccept = uc.IsAccept ? Constants.Accept : Constants.UnAccept,
                    AcceptanceDate = uc.AcceptanceDate == null ?
                        Constants.NotSet : PersianDateExtensions.ToPeString(uc.AcceptanceDate.Value, "yyyy/MM/dd HH:mm"),
                    CreationDate = PersianDateExtensions.ToPeString(uc.CreationDate, "yyyy/MM/dd HH:mm")

                }).SingleOrDefaultAsync();
        }

        public async Task<List<UserClassViewModel>> GetUserClasses(string userId = null, Guid? classGuid = null, bool? isAccept = null)
        {
            var userClasses = _unitOfWork.UserClasses.GetUserClasses(userId, classGuid, isAccept);
            
            return await userClasses
                .Select(uc => new UserClassViewModel
                {
                    UserClassId = uc.UserClassId,
                    UserClassGuid = uc.UserClassGuid,
                    User = new UserViewModel
                    {
                        Id = uc.User.Id,
                        FirstName = uc.User.FirstName,
                        LastName = uc.User.LastName,
                        PhoneNumber = uc.User.PhoneNumber,
                        RemainingTime = uc.User.RemainingTime.HasValue ? uc.User.RemainingTime.Value.ToString() : Constants.NotSet,
                        IsActive = uc.User.IsActive ? Constants.Active : Constants.InActive,
                        CreationDate = PersianDateExtensions.ToPeString(uc.CreationDate, "yyyy/MM/dd HH:mm"),
                        ModifiedDate = uc.User.ModifiedDate == null ?
                            Constants.NotSet : PersianDateExtensions.ToPeString(uc.User.ModifiedDate.Value, "yyyy/MM/dd HH:mm")
                    },
                    Class = new ClassViewModel
                    {
                        ClassId = uc.Class.ClassId,
                        ClassGuid = uc.Class.ClassGuid,
                        User = new UserViewModel
                        {
                            Id = uc.Class.User.Id,
                            FirstName = uc.Class.User.FirstName,
                            LastName = uc.Class.User.LastName,
                            PhoneNumber = uc.Class.User.PhoneNumber,
                            RemainingTime = uc.Class.User.RemainingTime.HasValue ? uc.User.RemainingTime.Value.ToString() : Constants.NotSet,
                            IsActive = uc.Class.User.IsActive ? Constants.Active : Constants.InActive,
                            CreationDate = PersianDateExtensions.ToPeString(uc.Class.User.CreationDate, "yyyy/MM/dd HH:mm"),
                            ModifiedDate = uc.Class.User.ModifiedDate == null ?
                                Constants.NotSet : PersianDateExtensions.ToPeString(uc.Class.User.ModifiedDate.Value, "yyyy/MM/dd HH:mm")
                        },
                        Title = uc.Class.Title,
                        Description = uc.Class.Description,
                        CreationDate = PersianDateExtensions.ToPeString(uc.Class.CreationDate, "yyyy/MM/dd HH:mm"),
                        ModifiedDate = uc.Class.ModifiedDate == null ?
                            Constants.NotSet : PersianDateExtensions.ToPeString(uc.Class.ModifiedDate.Value, "yyyy/MM/dd HH:mm")
                    },
                    IsAccept = uc.IsAccept ? Constants.Accept : Constants.UnAccept,
                    AcceptanceDate = uc.AcceptanceDate == null ?
                        Constants.NotSet : PersianDateExtensions.ToPeString(uc.AcceptanceDate.Value, "yyyy/MM/dd HH:mm"),
                    CreationDate = PersianDateExtensions.ToPeString(uc.CreationDate, "yyyy/MM/dd HH:mm")

                }).ToListAsync();
        }

        public async Task<int> GetUserClassesCount(string userId = null, Guid? classGuid = null, bool? isAccept = null)
        {
            return await _unitOfWork.UserClasses
                .GetUserClasses(userId, classGuid, isAccept)
                .CountAsync();
        }

        public async Task<bool> IsExists(string userId, Guid classGuid)
        {
            return await _unitOfWork.UserClasses
                .GetUserClass(userId, classGuid)
                .AnyAsync();
        }
    }
}
