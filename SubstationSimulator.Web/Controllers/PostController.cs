using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectricalEmulator.Application.Common;
using ElectricalEmulator.Application.Common.Extensions;
using ElectricalEmulator.Application.Interfaces;
using ElectricalEmulator.Application.ViewModels.Interlocks;
using ElectricalEmulator.Application.ViewModels.Posts;
using ElectricalEmulator.Application.ViewModels.Users;
using ElectricalEmulator.Domain.Enums;
using ElectricalEmulator.Infra.Data.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ElectricalEmulator.Web.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly ElectricalEmulatorContext _context;

        public PostController(IPostService postService,
            ElectricalEmulatorContext context)
        {
            _postService = postService;
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var postsViewModel = new List<PostViewModel>();

            var posts = await _context.Post
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.CreationDate)
                .ToListAsync();

            foreach (var post in posts)
            {
                var postJsonResultViewModel = JsonConvert.DeserializeObject<PostJsonResultViewModel>(post.RawValue);

                if (postJsonResultViewModel == null)
                    continue;

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

            return View(postsViewModel);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get(Guid postGuid)
        {
            var post = await _context.Post
                .SingleOrDefaultAsync(p => p.PostGuid.Equals(postGuid));

            if (post == null)
                return NoContent();

            var postJsonResultViewModel = JsonConvert.DeserializeObject<PostJsonResultViewModel>
                (post.RawValue);

            //var postElementsRegex = System.Text.RegularExpressions.Regex.Match(post.RawValue, @"(?:\""postElements\"" *: *)(\[[^\]]*\])");

            //if (!postElementsRegex.Success)
            //{
            //    return NoContent();
            //}

            //string postElements = postElementsRegex.Value;

            //for (int i = postElements.IndexOf('}'); i > -1; i = postElements.IndexOf('}', i + 1))
            //{
            //    postElements = postElements.Insert(i - 4, ",");
            //    postElements = postElements.Insert(i + 1, "\t\"test\": 0\r\n\t\t");
            //}

            if (postJsonResultViewModel == null)
                return NoContent();

            TempData["PostGuid"] = post.PostGuid;

            return View(postJsonResultViewModel);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetInterlocks(Guid interlockGuid)
        {
            if (interlockGuid == null)
                return BadRequest();

            var interlock = await _context.Interlock
                .SingleOrDefaultAsync(i => i.InterlockGuid == interlockGuid);

            if (interlock == null)
                return NotFound();

            var interlockJsonResultViewModel = JsonConvert.DeserializeObject<InterlocksJsonResultViewModel>
                (interlock.RawValue);

            if (interlockJsonResultViewModel == null)
                return NoContent();

            return PartialView(interlockJsonResultViewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetInfo()
        {
            return PartialView();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Delete(Guid postGuid)
        {
            TempData["PostGuid"] = postGuid;
            return PartialView();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete()
        {
            var result = await _postService.DeletePost((Guid)TempData["PostGuid"]);

            if (!result.Succeeded)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.DeletePostFailed;

                return RedirectToAction("Index");
            }

            TempData["ToasterState"] = ToasterState.Success;
            TempData["ToasterType"] = ToasterType.Message;
            TempData["ToasterMessage"] = Constants.DeletePostSuccessful;

            return RedirectToAction("Index");
        }
    }
}
