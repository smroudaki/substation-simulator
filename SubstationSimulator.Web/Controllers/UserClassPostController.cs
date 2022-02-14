using ElectricalEmulator.Application.Common;
using ElectricalEmulator.Application.Interfaces;
using ElectricalEmulator.Application.ViewModels.Posts;
using ElectricalEmulator.Application.ViewModels.UserClassPosts;
using ElectricalEmulator.Domain.Entities;
using ElectricalEmulator.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectricalEmulator.Web.Controllers
{
    public class UserClassPostController : Controller
    {
        private readonly IUserClassPostService _userClassPostService;
        private readonly IUserClassService _userClassService;
        private readonly IPostService _postService;

        public UserClassPostController(IUserClassPostService userClassPostService,
            IUserClassService userClassService,
            IPostService postService)
        {
            _userClassPostService = userClassPostService;
            _userClassService = userClassService;
            _postService = postService;
        }

        [Authorize(Roles = "Admin, Master, Student", Policy = "RemainigTime")]
        [HttpGet]
        public async Task<IActionResult> Index(CreateUserClassPostViewModel model)
        {
            string postChanges;

            var userClassPost = await _userClassPostService.GetUserClassPost(model.UserClassGuid, model.PostGuid);

            if (userClassPost == null)
            {
                var userClass = await _userClassService.GetUserClass(model.UserClassGuid);

                if (userClass == null)
                {
                    TempData["ToasterState"] = ToasterState.Error;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Constants.UserClassLoadError;

                    return RedirectToAction("MyClasses", "Student");
                }

                var post = await _postService.GetPost(model.PostGuid);

                if (post == null)
                {
                    TempData["ToasterState"] = ToasterState.Error;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Constants.PostLoadError;

                    return RedirectToAction("MyClasses", "Student");
                }

                var newUserClassPost = new UserClassPost
                {
                    UserClassId = userClass.UserClassId,
                    PostId = post.PostId,
                    PostChanges = post.RawValue
                };

                var result = await _userClassPostService.CreateUserClassPost(newUserClassPost);

                if (!result.Succeeded)
                {
                    TempData["ToasterState"] = ToasterState.Error;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Constants.PostCreateError;

                    return RedirectToAction("MyClasses", "Student");
                }

                postChanges = newUserClassPost.PostChanges;
            }
            else
            {
                postChanges = userClassPost.PostChanges;
            }

            var postJsonResultViewModel = JsonConvert.DeserializeObject<PostJsonResultViewModel>(postChanges);

            if (postJsonResultViewModel == null)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.PostDeserializeError;

                return RedirectToAction("MyClasses", "Student");
            }

            TempData["UserClassGuid"] = model.UserClassGuid;
            TempData["PostGuid"] = model.PostGuid;

            return View(postJsonResultViewModel);
        }

        [Authorize(Roles = "Admin, Master, Student")]
        [HttpGet]
        public async Task<string> GetPostChangesRaw(Guid userClassGuid, Guid postGuid)
        {
            var userClassPost = await _userClassPostService.GetUserClassPost(userClassGuid, postGuid);
            return userClassPost == null ? null : userClassPost.PostChanges;
        }

        [Authorize(Roles = "Admin, Master, Student")]
        [HttpPut]
        public async Task<bool> Update([FromQuery] Guid userClassGuid, [FromQuery] Guid postGuid, [FromBody] object userClassPostChangesRaw)
        {
            var result = await _userClassPostService.UpdateUserClassPost(userClassGuid, postGuid, userClassPostChangesRaw);
            return !result.Succeeded ? true : false;
        }
    }
}
