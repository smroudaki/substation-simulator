using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ElectricalEmulator.Application.Common;
using ElectricalEmulator.Application.Interfaces;
using ElectricalEmulator.Application.ViewModels.Posts;
using ElectricalEmulator.Application.ViewModels.UserPosts;
using ElectricalEmulator.Domain.Entities;
using ElectricalEmulator.Domain.Enums;
using ElectricalEmulator.Infra.Data.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ElectricalEmulator.Web.Controllers
{
    public class UserPostController : Controller
    {
        private readonly IUserPostService _userPostService;
        private readonly IPostService _postService;

        public UserPostController(IUserPostService userPostService,
            IPostService postService)
        {
            _userPostService = userPostService;
            _postService = postService;
        }

        [Authorize(Roles = "Admin, Master, Student", Policy = "RemainigTime")]
        [HttpGet]
        public async Task<IActionResult> Index(CreateUserPostViewModel model)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string postChanges;

            var userPost = await _userPostService.GetUserPost(currentUserId, model.PostGuid);

            if (userPost == null)
            {
                var post = await _postService.GetPost(model.PostGuid);

                if (post == null)
                {
                    TempData["ToasterState"] = ToasterState.Error;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Constants.PostLoadError;

                    return RedirectToAction("Index", "Post");
                }

                var newUserPost = new UserPost
                {
                    UserId = currentUserId,
                    PostId = post.PostId,
                    PostChanges = post.RawValue
                };

                var result = await _userPostService.CreateUserPost(newUserPost);

                if (!result.Succeeded)
                {
                    TempData["ToasterState"] = ToasterState.Error;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Constants.PostCreateError;

                    return RedirectToAction("Index", "Post");
                }

                postChanges = newUserPost.PostChanges;
            }
            else
            {
                postChanges = userPost.PostChanges;
            }

            var postJsonResultViewModel = JsonConvert.DeserializeObject<PostJsonResultViewModel>(postChanges);

            if (postJsonResultViewModel == null)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.PostDeserializeError;

                return RedirectToAction("Index", "Post");
            }

            TempData["PostGuid"] = model.PostGuid;

            return View(postJsonResultViewModel);
        }

        [Authorize(Roles = "Admin, Master, Student")]
        [HttpGet]
        public async Task<string> GetPostChangesRaw(Guid postGuid)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userPost = await _userPostService.GetUserPost(currentUserId, postGuid);
            return userPost == null ? null : userPost.PostChanges;
        }

        [Authorize(Roles = "Admin, Master, Student")]
        [HttpPut]
        public async Task<bool> Update([FromQuery] Guid postGuid, [FromBody] object userPostChangesRaw)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _userPostService.UpdateUserPost(currentUserId, postGuid, userPostChangesRaw);
            return !result.Succeeded ? true : false;
        }
    }
}
