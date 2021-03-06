using ElectricalEmulator.Application.Common;
using ElectricalEmulator.Application.Common.Extensions;
using ElectricalEmulator.Application.Common.ViewModels;
using ElectricalEmulator.Application.Interfaces;
using ElectricalEmulator.Application.ViewModels.Classes;
using ElectricalEmulator.Application.ViewModels.UserClasses;
using ElectricalEmulator.Application.ViewModels.Users;
using ElectricalEmulator.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Application.Services
{
    public class MasterService : IMasterService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MasterService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultViewModel> DeleteMaster(string userId)
        {
            var user = await _unitOfWork.Users
                .GetUser(userId)
                .SingleOrDefaultAsync();

            if (user == null)
            {
                return ResultViewModel.Failure();
            }

            var classes = await _unitOfWork.Classes
                .GetClasses(userId)
                .ToListAsync();

            foreach (var @class in classes)
            {
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
            }

            var userPosts = await _unitOfWork.UserPosts
                .GetUserPosts(userId)
                .ToListAsync();

            foreach (var userPost in userPosts)
            {
                _unitOfWork.UserPosts.Delete(userPost);
            }

            _unitOfWork.Users.Delete(user);

            await _unitOfWork.CommitAsync();

            return ResultViewModel.Success();
        }

        public async Task<List<UserClassViewModel>> GetRequests(string userId)
        {
            var classes = _unitOfWork.Classes.GetClasses(userId);
            var userClasses = _unitOfWork.UserClasses.GetUserClasses(classes, null, false);

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
                        CreationDate = PersianDateExtensions.ToPeString(uc.User.CreationDate, "yyyy/MM/dd HH:mm"),
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
                            RemainingTime = uc.User.RemainingTime.HasValue ? uc.User.RemainingTime.Value.ToString() : Constants.NotSet,
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
                    CreationDate = PersianDateExtensions.ToPeString(uc.CreationDate, "yyyy/MM/dd HH:mm"),
                    AcceptanceDate = uc.Class.ModifiedDate == null ?
                        Constants.NotSet : PersianDateExtensions.ToPeString(uc.Class.ModifiedDate.Value, "yyyy/MM/dd HH:mm")

                }).ToListAsync();
        }
    }
}
