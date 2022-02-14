using ElectricalEmulator.Application.Interfaces;
using ElectricalEmulator.Application.ViewModels.UserClassPosts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectricalEmulator.Web.Controllers
{
    public class ClassPostController : Controller
    {
        private readonly IPostService _postService;

        public ClassPostController(IPostService postService)
        {
            _postService = postService;
        }

        [Authorize(Roles = "Admin, Master, Student")]
        [HttpGet]
        public async Task<IActionResult> Index(Guid userClassGuid)
        {
            ViewBag.Posts = await _postService.GetPostsNameSelectListItem(true);

            var model = new CreateUserClassPostViewModel
            {
                UserClassGuid = userClassGuid
            };

            return PartialView(model);
        }
    }
}
