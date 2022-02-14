using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ElectricalEmulator.Application.Common;
using ElectricalEmulator.Application.Interfaces;
using ElectricalEmulator.Application.ViewModels.Accounts;
using ElectricalEmulator.Application.ViewModels.Admins;
using ElectricalEmulator.Application.ViewModels.UserClassPosts;
using ElectricalEmulator.Domain.Entities;
using ElectricalEmulator.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ElectricalEmulator.Web.Controllers
{
    public class MasterController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IMasterService _masterService;
        private readonly IClassService _classService;

        public MasterController(UserManager<User> userManager,
            IUserService userService,
            IMasterService masterService,
            IClassService classService)
        {
            _userManager = userManager;
            _userService = userService;
            _masterService = masterService;
            _classService = classService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var masters = await _userService.GetUsers(UserRole.Master);
            return View(masters);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Classes(string userId)
        {
            var classes = await _classService.GetClasses(userId);
            return View(classes);
        }

        [Authorize(Roles = "Master")]
        [HttpGet]
        public async Task<IActionResult> MyClasses()
        {
            var classes = await _classService.GetClasses(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(classes);
        }

        [Authorize(Roles = "Master")]
        [HttpGet]
        public async Task<IActionResult> MyRequests()
        {
            var myRequests = await _masterService.GetRequests(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(myRequests);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> CreateClass()
        {
            ViewBag.Users = await _userService.GetUsersPhoneNumberSelectListItem(UserRole.Master, true);
            return PartialView();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateClass(CreateClassViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.CreateClassFailed;

                return RedirectToAction("Index", "Class");
            }

            var @class = new Class
            {
                UserId = model.UserId,
                Title = model.Title.Trim(),
                Description = model.Description.Trim()
            };

            await _classService.CreateClass(@class);

            TempData["ToasterState"] = ToasterState.Success;
            TempData["ToasterType"] = ToasterType.Message;
            TempData["ToasterMessage"] = Constants.CreateClassSuccessful;

            return RedirectToAction("Index", "Class");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create() => PartialView();

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.CreateMasterFailed;

                return RedirectToAction("Index");
            }

            var user = new User
            {
                UserName = model.PhoneNumber,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            IdentityResult result;

            result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.CreateMasterFailed;

                return RedirectToAction("Index");
            }

            result = await _userManager.AddToRoleAsync(user, UserRole.Master.ToString());

            if (!result.Succeeded)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.CreateMasterFailed;

                return RedirectToAction("Index");
            }

            TempData["ToasterState"] = ToasterState.Success;
            TempData["ToasterType"] = ToasterType.Message;
            TempData["ToasterMessage"] = Constants.CreateMasterSuccessful;

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Delete(string userId)
        {
            TempData["UserId"] = userId;
            return PartialView();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete()
        {
            var result = await _masterService.DeleteMaster(TempData["UserId"].ToString());

            if (!result.Succeeded)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.MasterDeleteFailed;

                return RedirectToAction("Index");
            }

            TempData["ToasterState"] = ToasterState.Success;
            TempData["ToasterType"] = ToasterType.Message;
            TempData["ToasterMessage"] = Constants.MasterDeleteSuccessful;

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(string userId)
        {
            TempData["UserId"] = userId;
            return PartialView();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.ProfileEditFailed;

                return RedirectToAction("Index");
            }

            var result = await _userService.UpdateUser(TempData["UserId"].ToString(), model);

            if (!result.Succeeded)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.ProfileEditFailed;

                return RedirectToAction("Index");
            }

            TempData["ToasterState"] = ToasterState.Success;
            TempData["ToasterType"] = ToasterType.Message;
            TempData["ToasterMessage"] = Constants.ProfileEditSuccessful;

            return RedirectToAction("Index");
        }
    }
}
