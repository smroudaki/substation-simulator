using ElectricalEmulator.Application.Common;
using ElectricalEmulator.Application.Common.Extensions;
using ElectricalEmulator.Application.Common.ViewModels;
using ElectricalEmulator.Application.Interfaces;
using ElectricalEmulator.Application.ViewModels.Posts;
using ElectricalEmulator.Application.ViewModels.UserPosts;
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
    public class UserPostService : IUserPostService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserPostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultViewModel> CreateUserPost(UserPost userPost)
        {
            _unitOfWork.UserPosts.Insert(userPost);
            await _unitOfWork.CommitAsync();
            return ResultViewModel.Success();
        }

        public async Task<UserPostViewModel> GetUserPost(string userId, Guid postGuid)
        {
            return await _unitOfWork.UserPosts.GetUserPost(userId, postGuid)
                .Select(up => new UserPostViewModel
                {
                    UserPostId = up.UserPostId,
                    UserPostGuid = up.UserPostGuid,
                    User = new UserViewModel
                    {
                        Id = up.User.Id,
                        FirstName = up.User.FirstName,
                        LastName = up.User.LastName,
                        PhoneNumber = up.User.PhoneNumber,
                        RemainingTime = up.User.RemainingTime.HasValue ? up.User.RemainingTime.Value.ToString() : Constants.NotSet,
                        IsActive = up.User.IsActive ? Constants.Active : Constants.InActive,
                        CreationDate = PersianDateExtensions.ToPeString(up.User.CreationDate, "yyyy/MM/dd HH:mm"),
                        ModifiedDate = up.User.ModifiedDate == null ?
                            Constants.NotSet : PersianDateExtensions.ToPeString(up.User.ModifiedDate.Value, "yyyy/MM/dd HH:mm")

                    },
                    Post = new PostViewModel
                    {
                        PostId = up.Post.PostId,
                        PostGuid = up.Post.PostGuid,
                        RawValue = up.Post.RawValue,
                        RawValueDeserialized = new PostRawValueViewModel
                        {
                            
                        },
                        IsActive = up.Post.IsActive ? Constants.Active : Constants.InActive,
                        CreationDate = PersianDateExtensions.ToPeString(up.Post.CreationDate, "yyyy/MM/dd HH:mm"),
                        ModifiedDate = up.Post.ModifiedDate == null ?
                            Constants.NotSet : PersianDateExtensions.ToPeString(up.Post.ModifiedDate.Value, "yyyy/MM/dd HH:mm")
                    },
                    PostChanges = up.PostChanges,
                    CreationDate = PersianDateExtensions.ToPeString(up.CreationDate, "yyyy/MM/dd HH:mm"),
                    ModifiedDate = up.ModifiedDate == null ?
                        Constants.NotSet : PersianDateExtensions.ToPeString(up.ModifiedDate.Value, "yyyy/MM/dd HH:mm")

                }).FirstOrDefaultAsync();
        }

        public async Task<ResultViewModel> UpdateUserPost(string userId, Guid postGuid, object userPostChangesRaw)
        {
            var userPost = await _unitOfWork.UserPosts
                .GetUserPost(userId, postGuid)
                .FirstOrDefaultAsync();

            if (userPost == null)
            {
                return ResultViewModel.Failure();
            }

            userPost.PostChanges = userPostChangesRaw.ToString();
            userPost.ModifiedDate = DateTime.Now;

            await _unitOfWork.CommitAsync();

            return ResultViewModel.Success();
        }
    }
}
