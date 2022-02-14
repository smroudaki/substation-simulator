using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ElectricalEmulator.Application.Common;
using ElectricalEmulator.Application.Interfaces;
using ElectricalEmulator.Application.ViewModels.Accounts;
using ElectricalEmulator.Domain.Entities;
using ElectricalEmulator.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ElectricalEmulator.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;
        private readonly ISettingService _settingService;

        public AccountController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IUserService userService,
            ISettingService settingService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _settingService = settingService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Dashboard");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Username or password is not correct.");
                return View(model);
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Dashboard");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var settings = await _settingService.GetSettings();

            if (settings == null)
            {
                return View(model);
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
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            result = await _userManager.AddToRoleAsync(user, "Student");

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Dashboard");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult ChangePassword() => PartialView();

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.PasswordChangeFailed;

                return RedirectToAction("Index", "Dashboard");
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.PasswordChangeFailed;

                return RedirectToAction("Index", "Dashboard");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.PasswordChangeFailed;

                return RedirectToAction("Index", "Dashboard");
            }

            TempData["ToasterState"] = ToasterState.Success;
            TempData["ToasterType"] = ToasterType.Message;
            TempData["ToasterMessage"] = Constants.PasswordChangeSuccessful;

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public IActionResult EditProfile() => PartialView();

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.ProfileEditFailed;

                return RedirectToAction("Index", "Dashboard");
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _userService.UpdateUser(currentUserId, model);

            if (!result.Succeeded)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.ProfileEditFailed;

                return RedirectToAction("Index", "Dashboard");
            }

            await _signInManager.SignOutAsync();

            var user = await _userManager.GetUserAsync(User);
            await _signInManager.SignInAsync(user, false);

            TempData["ToasterState"] = ToasterState.Success;
            TempData["ToasterType"] = ToasterType.Message;
            TempData["ToasterMessage"] = Constants.ProfileEditSuccessful;

            return RedirectToAction("Index", "Dashboard");
        }
    }
}
