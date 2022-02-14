using ElectricalEmulator.Application.Common;
using ElectricalEmulator.Application.Interfaces;
using ElectricalEmulator.Application.ViewModels.Accounts;
using ElectricalEmulator.Domain.Entities;
using ElectricalEmulator.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ElectricalEmulator.Web.Controllers
{
    public class StudentController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IStudentService _studentService;
        private readonly IClassService _classService;
        private readonly IUserClassService _userClassService;
        private readonly ISettingService _settingService;

        public StudentController(UserManager<User> userManager,
            IUserService userService,
            IStudentService studentService,
            IClassService classService,
            IUserClassService userClassService,
            ISettingService settingService)
        {
            _userManager = userManager;
            _userService = userService;
            _studentService = studentService;
            _classService = classService;
            _userClassService = userClassService;
            _settingService = settingService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var students = await _userService.GetUsers(UserRole.Student);
            return View(students);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Classes(string userId)
        {
            var userClasses = await _userClassService.GetUserClasses(userId);
            return View(userClasses);
        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        public async Task<IActionResult> MyClasses()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userClasses = await _userClassService.GetUserClasses(currentUserId);
            return View(userClasses);
        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        public async Task<IActionResult> AvailableClasses()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var classes = await _classService.GetAvailableClasses(currentUserId);
            return View(classes);
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
                TempData["ToasterMessage"] = Constants.CreateStudentFailed;

                return RedirectToAction("Index");
            }

            var settings = await _settingService.GetSettings();

            if (settings == null)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.CreateStudentFailed;

                return RedirectToAction("Index");
            }

            var user = new User
            {
                UserName = model.PhoneNumber,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                RemainingTime = settings.UserInitialRemainingTime
            };

            IdentityResult result;

            result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.CreateStudentFailed;

                return RedirectToAction("Index");
            }

            result = await _userManager.AddToRoleAsync(user, UserRole.Student.ToString());

            if (!result.Succeeded)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.CreateStudentFailed;

                return RedirectToAction("Index");
            }

            TempData["ToasterState"] = ToasterState.Success;
            TempData["ToasterType"] = ToasterType.Message;
            TempData["ToasterMessage"] = Constants.CreateStudentSuccessful;

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
            var result = await _studentService.DeleteStudent(TempData["UserId"].ToString());

            if (!result.Succeeded)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.StudentDeleteFailed;

                return RedirectToAction("Index");
            }

            TempData["ToasterState"] = ToasterState.Success;
            TempData["ToasterType"] = ToasterType.Message;
            TempData["ToasterMessage"] = Constants.StudentDeleteSuccessful;

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
