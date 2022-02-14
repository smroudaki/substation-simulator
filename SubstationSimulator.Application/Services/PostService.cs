using ElectricalEmulator.Application.Common;
using ElectricalEmulator.Application.Common.Extensions;
using ElectricalEmulator.Application.Common.ViewModels;
using ElectricalEmulator.Application.Interfaces;
using ElectricalEmulator.Application.ViewModels.Posts;
using ElectricalEmulator.Application.ViewModels.Users;
using ElectricalEmulator.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElectricalEmulator.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultViewModel> DeletePost(Guid postGuid)
        {
            var post = await _unitOfWork.Posts
                .GetPost(postGuid)
                .SingleOrDefaultAsync();

            if (post == null)
            {
                return ResultViewModel.Failure();
            }

            var userClassPosts = await _unitOfWork.UserClassPosts
                .GetUserClassPosts(null, postGuid)
                .ToListAsync();

            foreach (var userClassPost in userClassPosts)
            {
                _unitOfWork.UserClassPosts.Delete(userClassPost);
            }

            var userPosts = await _unitOfWork.UserPosts
                .GetUserPosts(null, postGuid)
                .ToListAsync();

            foreach (var userPost in userPosts)
            {
                _unitOfWork.UserPosts.Delete(userPost);
            }

            var interlockGuidsRegex = Regex.Matches(post.RawValue, "\"interlockGuid\":\\s*\"(.*)\"");

            for (int i = 0; i < interlockGuidsRegex.Count; i++)
            {
                Guid.TryParse(interlockGuidsRegex[i].Groups[1].Value, out Guid interlockGuid);

                if (interlockGuid == null)
                {
                    continue;
                }

                var interlock = await _unitOfWork.Interlocks
                    .GetInterlock(interlockGuid)
                    .SingleOrDefaultAsync();

                if (interlock == null)
                {
                    continue;
                }

                _unitOfWork.Interlocks.Delete(interlock);
            }

            _unitOfWork.Posts.Delete(post);

            await _unitOfWork.CommitAsync();

            return ResultViewModel.Success();
        }

        public async Task<PostViewModel> GetPost(Guid postGuid)
        {
            var post = await _unitOfWork.Posts
                .GetPost(postGuid)
                .SingleOrDefaultAsync();

            if (post == null)
            {
                return null;
            }

            //var postJsonResultViewModel = JsonConvert.DeserializeObject<PostJsonResultViewModel>(post.RawValue);

            //if (postJsonResultViewModel == null)
            //{
            //    return null;
            //}

            return new PostViewModel
            {
                PostId = post.PostId,
                PostGuid = post.PostGuid,
                RawValue = post.RawValue,
                RawValueDeserialized = new PostRawValueViewModel
                {
                    
                },
                IsActive = post.IsActive ? Constants.Active : Constants.InActive,
                CreationDate = PersianDateExtensions.ToPeString(post.CreationDate, "yyyy/MM/dd HH:mm"),
                ModifiedDate = post.ModifiedDate == null ?
                    Constants.NotSet : PersianDateExtensions.ToPeString(post.ModifiedDate.Value, "yyyy/MM/dd HH:mm")
            };
        }

        public async Task<List<PostViewModel>> GetPosts(bool? isActive = null)
        {
            var posts = await _unitOfWork.Posts
                .GetPosts(isActive)
                .ToListAsync();

            var postsViewModel = new List<PostViewModel>();

            foreach (var post in posts)
            {
                var postJsonResultViewModel = JsonConvert.DeserializeObject<PostJsonResultViewModel>(post.RawValue);

                if (postJsonResultViewModel == null)
                {
                    continue;
                }

                var postViewModel = new PostViewModel
                {
                    PostId = post.PostId,
                    PostGuid = post.PostGuid,
                    RawValue = post.RawValue,
                    RawValueDeserialized = new PostRawValueViewModel
                    {
                        Name = postJsonResultViewModel.Name,
                    },
                    IsActive = post.IsActive ? Constants.Active : Constants.InActive,
                    CreationDate = PersianDateExtensions.ToPeString(post.CreationDate, "yyyy/MM/dd HH:mm"),
                    ModifiedDate = post.ModifiedDate == null ?
                        Constants.NotSet : PersianDateExtensions.ToPeString(post.ModifiedDate.Value, "yyyy/MM/dd HH:mm")
                };

                postsViewModel.Add(postViewModel);
            }

            return postsViewModel;
        }

        public async Task<int> GetPostsCount(bool? isActive = null)
        {
            return await _unitOfWork.Posts
                .GetPosts(isActive)
                .CountAsync();
        }

        public async Task<List<SelectListItem>> GetPostsNameSelectListItem(bool? isActive = null)
        {
            var posts = await _unitOfWork.Posts
                .GetPosts(isActive)
                .ToListAsync();

            var postsViewModel = new List<SelectListItem>();

            foreach (var post in posts)
            {
                var postJsonResultViewModel = JsonConvert.DeserializeObject<PostJsonResultViewModel>(post.RawValue);

                if (postJsonResultViewModel == null)
                {
                    continue;
                }

                var postViewModel = new SelectListItem
                {
                    Value = post.PostGuid.ToString(),
                    Text = postJsonResultViewModel.Name
                };

                postsViewModel.Add(postViewModel);
            }

            return postsViewModel;
        }

        public async Task<ResultViewModel> UpdatePost(Guid postGuid, object rawValue)
        {
            var post = await _unitOfWork.Posts
                .GetPost(postGuid)
                .SingleOrDefaultAsync();

            if (post == null)
            {
                return ResultViewModel.Failure();
            }

            var interlockGuidsRegex = Regex.Matches(post.RawValue, "\"interlockGuid\":\\s*\"(.*)\"");

            for (int i = 0; i < interlockGuidsRegex.Count; i++)
            {
                Guid.TryParse(interlockGuidsRegex[i].Groups[1].Value, out Guid interlockGuid);

                if (interlockGuid == null)
                {
                    continue;
                }

                var interlock = await _unitOfWork.Interlocks
                    .GetInterlock(interlockGuid)
                    .SingleOrDefaultAsync();

                if (interlock == null)
                {
                    continue;
                }

                _unitOfWork.Interlocks.Delete(interlock);
            }

            var now = DateTime.Now;

            post.RawValue = rawValue.ToString();
            post.ModifiedDate = now;

            var userClassPosts = await _unitOfWork.UserClassPosts
                .GetUserClassPosts(null, postGuid)
                .ToListAsync();

            foreach (var userClassPost in userClassPosts)
            {
                userClassPost.PostChanges = post.RawValue;
                userClassPost.ModifiedDate = now;
            }

            var userPosts = await _unitOfWork.UserPosts
                .GetUserPosts(null, postGuid)
                .ToListAsync();

            foreach (var userPost in userPosts)
            {
                userPost.PostChanges = post.RawValue;
                userPost.ModifiedDate = now;
            }

            await _unitOfWork.CommitAsync();

            return ResultViewModel.Success();
        }
    }
}
