using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ElectricalEmulator.Application.Interfaces;
using ElectricalEmulator.Application.ViewModels.Posts;
using ElectricalEmulator.Domain.Entities;
using ElectricalEmulator.Infra.Data.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ElectricalEmulator.Web.Controllers.Api
{
    public class PostController : BaseApiController
    {
        private readonly ElectricalEmulatorContext _context;
        private readonly IPostService _postService;

        public PostController(ElectricalEmulatorContext context,
            IPostService postService)
        {
            _context = context;
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = new PostsNameViewModel();

            var posts = await _context.Post
                .OrderByDescending(p => p.CreationDate)
                .ToListAsync();

            foreach (var post in posts)
            {
                var postJsonResultViewModel = JsonConvert.DeserializeObject<PostJsonResultViewModel>(post.RawValue);

                if (postJsonResultViewModel == null)
                {
                    continue;
                }

                var postNameViewModel = new PostNameViewModel
                {
                    PostGuid = post.PostGuid,
                    Name = postJsonResultViewModel.Name
                };

                model.Posts.Add(postNameViewModel);
            }

            return Ok(model);
        }

        [HttpGet("{postGuid}")]
        public async Task<IActionResult> GetById(Guid postGuid)
        {
            if (postGuid == null)
            {
                return BadRequest();
            }

            var postRaw = await _context.Post
                .Where(p => p.PostGuid.Equals(postGuid))
                .Select(p => p.RawValue)
                .SingleOrDefaultAsync();

            if (postRaw == null)
            {
                return NotFound();
            }

            var interlockGuidsRegex = Regex.Matches(postRaw, "\"interlockGuid\":\\s*\"(.*)\"");

            for (int i = 0; i < interlockGuidsRegex.Count; i++)
            {
                Guid.TryParse(interlockGuidsRegex[i].Groups[1].Value, out Guid interlockGuid);

                if (interlockGuid == null)
                {
                    continue;
                }

                var interlockRaw = await _context.Interlock
                    .Where(i => i.InterlockGuid.Equals(interlockGuid))
                    .Select(i => i.RawValue)
                    .SingleOrDefaultAsync();

                if (interlockRaw == null)
                {
                    continue;
                }

                interlockRaw = Regex.Match(interlockRaw, "{\\s*\"interlocks\":\\s*(\\[[^\\]]*\\])\\s*}").Groups[1].Value;

                interlockRaw = Regex.Replace(interlockRaw, "((?:\\t)+)", "$1\t\t");

                postRaw = Regex.Replace(postRaw, "\"interlockGuid\":\\s*\"" + interlockGuid.ToString() + "\"", "\"interlocks\": " + interlockRaw);
            }

            return Ok(postRaw);

            #region Previous code

            //var post = await _context.Post
            //    .SingleOrDefaultAsync(p => p.PostGuid.Equals(postGuid));

            //if (post == null)
            //{
            //    return NotFound();
            //}

            //var postJsonResultViewModel = JsonConvert.DeserializeObject<PostJsonResultViewModel>
            //    (post.RawValue);

            //if (postJsonResultViewModel == null)
            //    return NoContent();

            //var postViewModel = new PostViewModel
            //{
            //    Name = postJsonResultViewModel.Name
            //};

            //foreach (var postElement in postJsonResultViewModel.PostElements)
            //{
            //    var postElementViewModel = new PostElementViewModel
            //    {
            //        Name = postElement.Name,
            //        TypeCode = postElement.TypeCode,
            //        LocX = postElement.LocX,
            //        LocY = postElement.LocY,
            //        RotZ = postElement.RotZ
            //    };

            //    if (postElement.InterlockGuid == null)
            //    {
            //        postViewModel.PostElements.Add(postElementViewModel);
            //        continue;
            //    }

            //    var interlocks = await _context.Interlock
            //        .SingleOrDefaultAsync(i => i.InterlockGuid == postElement.InterlockGuid);

            //    if (interlocks == null)
            //    {
            //        postViewModel.PostElements.Add(postElementViewModel);
            //        continue;
            //    }

            //    var interlockJsonResultViewModel = JsonConvert.DeserializeObject<InterlocksJsonResultViewModel>
            //        (interlocks.RawValue);

            //    foreach (var interlock in interlockJsonResultViewModel.Interlocks)
            //    {
            //        var interlockViewModel = new InterlockViewModel
            //        {
            //            Name = interlock.Name,
            //            TypeCode = interlock.TypeCode,
            //            LocX = interlock.LocX,
            //            LocY = interlock.LocY,
            //            RotZ = interlock.RotZ
            //        };

            //        postElementViewModel.Interlocks.Add(interlockViewModel);
            //    }

            //    postViewModel.PostElements.Add(postElementViewModel);
            //}

            //return Ok(postViewModel);

            #endregion
        }

        [HttpPost]
        public async Task<IActionResult> Create(object rawData)
        {
            if (rawData == null)
            {
                return BadRequest();
            }

            var post = new Post
            {
                RawValue = rawData.ToString()
            };

            _context.Post.Add(post);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), post.PostGuid.ToString());
        }

        [HttpPut("{postGuid}")]
        public async Task<IActionResult> Update(Guid postGuid, object rawData)
        {
            var result = await _postService.UpdatePost(postGuid, rawData);

            if (!result.Succeeded)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
