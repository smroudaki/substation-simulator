using ElectricalEmulator.Application.Common;
using ElectricalEmulator.Application.Common.Extensions;
using ElectricalEmulator.Application.Common.ViewModels;
using ElectricalEmulator.Application.Interfaces;
using ElectricalEmulator.Application.ViewModels.Classes;
using ElectricalEmulator.Application.ViewModels.Posts;
using ElectricalEmulator.Application.ViewModels.UserClasses;
using ElectricalEmulator.Application.ViewModels.UserClassPosts;
using ElectricalEmulator.Application.ViewModels.Users;
using ElectricalEmulator.Domain.Entities;
using ElectricalEmulator.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Application.Services
{
    public class UserClassPostService : IUserClassPostService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserClassPostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultViewModel> CreateUserClassPost(UserClassPost userClassPost)
        {
            _unitOfWork.UserClassPosts.Insert(userClassPost);
            await _unitOfWork.CommitAsync();
            return ResultViewModel.Success();
        }

        public async Task<UserClassPostViewModel> GetUserClassPost(Guid userClassPostGuid, Guid postGuid)
        {
            return await _unitOfWork.UserClassPosts.GetUserClassPost(userClassPostGuid, postGuid)
                .Select(ucp => new UserClassPostViewModel
                {
                    UserClassPostId = ucp.UserClassPostId,
                    UserClassPostGuid = ucp.UserClassPostGuid,
                    UserClass = new UserClassViewModel
                    {
                        UserClassId = ucp.UserClass.UserClassId,
                        UserClassGuid = ucp.UserClass.UserClassGuid,
                        User = new UserViewModel
                        {
                            Id = ucp.UserClass.User.Id,
                            FirstName = ucp.UserClass.User.FirstName,
                            LastName = ucp.UserClass.User.LastName,
                            PhoneNumber = ucp.UserClass.User.PhoneNumber,
                            RemainingTime = ucp.UserClass.User.RemainingTime.HasValue ? ucp.UserClass.User.RemainingTime.Value.ToString() : Constants.NotSet,
                            IsActive = ucp.UserClass.User.IsActive ? Constants.Active : Constants.InActive,
                            CreationDate = PersianDateExtensions.ToPeString(ucp.CreationDate, "yyyy/MM/dd HH:mm"),
                            ModifiedDate = ucp.UserClass.User.ModifiedDate == null ?
                            Constants.NotSet : PersianDateExtensions.ToPeString(ucp.UserClass.User.ModifiedDate.Value, "yyyy/MM/dd HH:mm")
                        },
                        Class = new ClassViewModel
                        {
                            ClassId = ucp.UserClass.Class.ClassId,
                            ClassGuid = ucp.UserClass.Class.ClassGuid,
                            User = new UserViewModel
                            {
                                Id = ucp.UserClass.Class.User.Id,
                                FirstName = ucp.UserClass.Class.User.FirstName,
                                LastName = ucp.UserClass.Class.User.LastName,
                                PhoneNumber = ucp.UserClass.Class.User.PhoneNumber,
                                RemainingTime = ucp.UserClass.User.RemainingTime.HasValue ? ucp.UserClass.User.RemainingTime.Value.ToString() : Constants.NotSet,
                                IsActive = ucp.UserClass.Class.User.IsActive ? Constants.Active : Constants.InActive,
                                CreationDate = PersianDateExtensions.ToPeString(ucp.UserClass.Class.User.CreationDate, "yyyy/MM/dd HH:mm"),
                                ModifiedDate = ucp.UserClass.Class.User.ModifiedDate == null ?
                                    Constants.NotSet : PersianDateExtensions.ToPeString(ucp.UserClass.Class.User.ModifiedDate.Value, "yyyy/MM/dd HH:mm")
                            },
                            Title = ucp.UserClass.Class.Title,
                            Description = ucp.UserClass.Class.Description,
                            CreationDate = PersianDateExtensions.ToPeString(ucp.UserClass.Class.CreationDate, "yyyy/MM/dd HH:mm"),
                            ModifiedDate = ucp.UserClass.Class.ModifiedDate == null ?
                            Constants.NotSet : PersianDateExtensions.ToPeString(ucp.UserClass.Class.ModifiedDate.Value, "yyyy/MM/dd HH:mm")
                        },
                        IsAccept = ucp.UserClass.IsAccept ? Constants.Accept : Constants.UnAccept,
                        AcceptanceDate = ucp.UserClass.AcceptanceDate == null ?
                            Constants.NotSet : PersianDateExtensions.ToPeString(ucp.UserClass.AcceptanceDate.Value, "yyyy/MM/dd HH:mm"),
                        CreationDate = PersianDateExtensions.ToPeString(ucp.CreationDate, "yyyy/MM/dd HH:mm")

                    },
                    Post = new PostViewModel
                    {
                        PostId = ucp.Post.PostId,
                        PostGuid = ucp.Post.PostGuid,
                        RawValue = ucp.Post.RawValue,
                        RawValueDeserialized = new PostRawValueViewModel
                        {
                            
                        },
                        IsActive = ucp.Post.IsActive ? Constants.Active : Constants.InActive,
                        CreationDate = PersianDateExtensions.ToPeString(ucp.Post.CreationDate, "yyyy/MM/dd HH:mm"),
                        ModifiedDate = ucp.Post.ModifiedDate == null ?
                            Constants.NotSet : PersianDateExtensions.ToPeString(ucp.Post.ModifiedDate.Value, "yyyy/MM/dd HH:mm")
                    },
                    PostChanges = ucp.PostChanges,
                    CreationDate = PersianDateExtensions.ToPeString(ucp.CreationDate, "yyyy/MM/dd HH:mm"),
                    ModifiedDate = ucp.ModifiedDate == null ?
                        Constants.NotSet : PersianDateExtensions.ToPeString(ucp.ModifiedDate.Value, "yyyy/MM/dd HH:mm")

                }).FirstOrDefaultAsync();
        }

        public async Task<ResultViewModel> UpdateUserClassPost(Guid userClassGuid, Guid postGuid, object userClassPostChangesRaw)
        {
            var userClassPost = await _unitOfWork.UserClassPosts
                .GetUserClassPost(userClassGuid, postGuid)
                .FirstOrDefaultAsync();

            if (userClassPost == null)
            {
                return ResultViewModel.Failure();
            }

            userClassPost.PostChanges = userClassPostChangesRaw.ToString();
            userClassPost.ModifiedDate = DateTime.Now;

            await _unitOfWork.CommitAsync();

            return ResultViewModel.Success();
        }
    }
}
