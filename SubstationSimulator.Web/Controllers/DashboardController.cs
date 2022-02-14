using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ElectricalEmulator.Application.Interfaces;
using ElectricalEmulator.Application.ViewModels.Interlocks;
using ElectricalEmulator.Application.ViewModels.PostElements;
using ElectricalEmulator.Application.ViewModels.Posts;
using ElectricalEmulator.Domain.Enums;
using ElectricalEmulator.Infra.Data.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ElectricalEmulator.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IClassService _classService;
        private readonly IUserClassService _userClassService;
        private readonly IPostService _postService;
        private readonly IUserService _userService;

        public DashboardController(IClassService classService,
            IUserClassService userClassService,
            IPostService postService,
            IUserService userService)
        {
            _classService = classService;
            _userClassService = userClassService;
            _postService = postService;
            _userService = userService;
        }

        [Authorize(Roles = "Admin, Master, Student")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUserRole = User.FindFirstValue(ClaimTypes.Role);

            if (currentUserRole.Equals(UserRole.Admin.ToString()))
            {
                ViewBag.ClassesCount = await _classService.GetClassesCount();
                ViewBag.PostsCount = await _postService.GetPostsCount();
                ViewBag.MastersCount = await _userService.GetUsersCount(UserRole.Master);
                ViewBag.StudentsCount = await _userService.GetUsersCount(UserRole.Student);
            }
            else if (currentUserRole.Equals(UserRole.Master.ToString()))
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                ViewBag.MyClassesCount = await _classService.GetClassesCount(currentUserId);
            }
            else
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                ViewBag.MyClassesCount = await _userClassService.GetUserClassesCount(currentUserId, null, true);
                ViewBag.MyRequestsCount = await _userClassService.GetUserClassesCount(currentUserId, null, false);
            }
            
            return View();
        }
    }
}
