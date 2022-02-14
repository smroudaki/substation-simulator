using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ElectricalEmulator.Application.Common;
using ElectricalEmulator.Application.Interfaces;
using ElectricalEmulator.Application.ViewModels.Classes;
using ElectricalEmulator.Application.ViewModels.UserClasses;
using ElectricalEmulator.Application.ViewModels.UserClassPosts;
using ElectricalEmulator.Domain.Entities;
using ElectricalEmulator.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectricalEmulator.Web.Controllers
{
    public class ClassController : Controller
    {
        private readonly IUserService _userService;
        private readonly IClassService _classService;
        private readonly IUserClassService _userClassService;
        private readonly IPostService _postService;

        public ClassController(IUserService userService,
            IClassService classService,
            IUserClassService userClassService,
            IPostService postService)
        {
            _userService = userService;
            _classService = classService;
            _userClassService = userClassService;
            _postService = postService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var classes = await _classService.GetClasses();
            return View(classes);
        }

        [Authorize(Roles = "Admin, Master")]
        [HttpGet]
        public async Task<IActionResult> Students(Guid classGuid)
        {
            if (User.FindFirstValue(ClaimTypes.Role).Equals(UserRole.Master.ToString()))
            {
                var @class = await _classService.GetClass(classGuid);

                if (@class == null || !User.FindFirstValue(ClaimTypes.NameIdentifier).Equals(@class.User.Id))
                {
                    return View(new List<UserClassPostViewModel>());
                }
            }

            var classes = await _userClassService.GetUserClasses(null, classGuid, true);

            TempData["ClassGuid"] = classGuid;

            return View(classes);
        }

        [Authorize(Roles = "Admin, Master")]
        [HttpGet]
        public async Task<IActionResult> Posts(Guid userClassGuid)
        {
            ViewBag.Posts = await _postService.GetPostsNameSelectListItem(true);

            var model = new CreateUserClassPostViewModel
            {
                UserClassGuid = userClassGuid
            };

            return PartialView(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Requests()
        {
            var userClasses = await _userClassService.GetUserClasses(null, null, false);
            return PartialView(userClasses);
        }

        [Authorize(Roles = "Master")]
        [HttpGet]
        public IActionResult Create() => PartialView();

        [Authorize(Roles = "Master")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateClassViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.CreateClassFailed;

                return RedirectToAction("MyClasses", "Master");
            }

            var @class = new Class
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Title = model.Title.Trim(),
                Description = model.Description.Trim()
            };

            await _classService.CreateClass(@class);

            TempData["ToasterState"] = ToasterState.Success;
            TempData["ToasterType"] = ToasterType.Message;
            TempData["ToasterMessage"] = Constants.CreateClassSuccessful;

            return RedirectToAction("MyClasses", "Master");
        }

        [Authorize(Roles = "Admin, Master")]
        [HttpGet]
        public async Task<IActionResult> Register(Guid classGuid)
        {
            ViewBag.Users = await _userService.GetUsersPhoneNumberSelectListItem(UserRole.Student, true);

            var model = new RegisterViewModel
            {
                ClassGuid = classGuid
            };

            return PartialView(model);
        }

        [Authorize(Roles = "Admin, Master")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.ClassRegisterFailed;

                return RedirectToAction("Students", new { classGuid = model.ClassGuid });
            }

            var @class = await _classService.GetClass(model.ClassGuid);

            if (@class == null)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.ClassRegisterFailed;

                return RedirectToAction("Students", new { classGuid = model.ClassGuid });
            }

            if (await _userClassService.IsExists(model.UserId, model.ClassGuid))
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.ClassRegisterExists;

                return RedirectToAction("Students", new { classGuid = model.ClassGuid });
            }

            var userClass = new UserClass
            {
                UserId = model.UserId,
                ClassId = @class.ClassId,
                IsAccept = true,
                AcceptanceDate = DateTime.Now
            };

            await _userClassService.CreateUserClass(userClass);

            TempData["ToasterState"] = ToasterState.Success;
            TempData["ToasterType"] = ToasterType.Message;
            TempData["ToasterMessage"] = Constants.ClassRegisterSuccessful;

            return RedirectToAction("Students", new { classGuid = model.ClassGuid });
        }

        [Authorize(Roles = "Admin, Master")]
        [HttpGet]
        public IActionResult UnRegister(Guid classGuid, Guid userClassGuid)
        {
            TempData["ClassGuid"] = classGuid;
            TempData["UserClassGuid"] = userClassGuid;
            return PartialView();
        }

        [Authorize(Roles = "Admin, Master")]
        [HttpPost]
        public async Task<IActionResult> UnRegister()
        {
            var result = await _userClassService.DeleteUserClass((Guid)TempData["UserClassGuid"]);

            if (!result.Succeeded)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.ClassUnregisterFailed;

                return RedirectToAction("Students", new { ClassGuid = TempData["ClassGuid"] });
            }

            TempData["ToasterState"] = ToasterState.Success;
            TempData["ToasterType"] = ToasterType.Message;
            TempData["ToasterMessage"] = Constants.ClassUnregisterSuccessful;

            return RedirectToAction("Students", new { ClassGuid = TempData["ClassGuid"] });
        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        public IActionResult SendRequest(Guid classGuid)
        {
            var model = new SendRequestViewModel
            {
                ClassGuid = classGuid
            };

            return PartialView(model);
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        public async Task<IActionResult> SendRequest(SendRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.ClassSendRequestFailed;

                return RedirectToAction("AvailableClasses", "Student");
            }

            var @class = await _classService.GetClass(model.ClassGuid);

            if (@class == null)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.ClassSendRequestFailed;

                return RedirectToAction("AvailableClasses", "Student");
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (await _userClassService.IsExists(currentUserId, model.ClassGuid))
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.ClassRegisterExists;

                return RedirectToAction("AvailableClasses", "Student");
            }

            var userClass = new UserClass
            {
                UserId = currentUserId,
                ClassId = @class.ClassId
            };

            await _userClassService.CreateUserClass(userClass);

            TempData["ToasterState"] = ToasterState.Success;
            TempData["ToasterType"] = ToasterType.Message;
            TempData["ToasterMessage"] = Constants.ClassSendRequestSuccessful;

            return RedirectToAction("AvailableClasses", "Student");
        }

        [Authorize(Roles = "Admin, Master")]
        [HttpGet]
        public IActionResult AcceptRequest(Guid userClassGuid)
        {
            TempData["UserClassGuid"] = userClassGuid;
            return PartialView();
        }

        [Authorize(Roles = "Admin, Master")]
        [HttpPost]
        public async Task<IActionResult> AcceptRequest()
        {
            var result = await _userClassService.AcceptUserClass((Guid)TempData["UserClassGuid"]);

            if (!result.Succeeded)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.UserClassAcceptRequestFailed;

                if (User.FindFirstValue(ClaimTypes.Role).Equals(UserRole.Admin.ToString()))
                {
                    return RedirectToAction("Requests");
                }

                return RedirectToAction("MyRequests", "Master");
            }

            TempData["ToasterState"] = ToasterState.Success;
            TempData["ToasterType"] = ToasterType.Message;
            TempData["ToasterMessage"] = Constants.UserClassAcceptRequestSuccessful;

            if (User.FindFirstValue(ClaimTypes.Role).Equals(UserRole.Admin.ToString()))
            {
                return RedirectToAction("Requests");
            }

            return RedirectToAction("MyRequests", "Master");
        }

        [Authorize(Roles = "Admin, Master")]
        [HttpGet]
        public IActionResult DeleteRequest(Guid userClassGuid)
        {
            TempData["UserClassGuid"] = userClassGuid;
            return PartialView();
        }

        [Authorize(Roles = "Admin, Master")]
        [HttpPost]
        public async Task<IActionResult> DeleteRequest()
        {
            var result = await _userClassService.DeleteUserClass((Guid)TempData["UserClassGuid"]);

            if (!result.Succeeded)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.UserClassDeleteRequestFailed;

                if (User.FindFirstValue(ClaimTypes.Role).Equals(UserRole.Admin.ToString()))
                {
                    return RedirectToAction("Requests");
                }

                return RedirectToAction("MyRequests", "Master");
            }

            TempData["ToasterState"] = ToasterState.Success;
            TempData["ToasterType"] = ToasterType.Message;
            TempData["ToasterMessage"] = Constants.UserClassDeleteRequestSuccessful;

            if (User.FindFirstValue(ClaimTypes.Role).Equals(UserRole.Admin.ToString()))
            {
                return RedirectToAction("Requests");
            }

            return RedirectToAction("MyRequests", "Master");
        }

        [Authorize(Roles = "Admin, Master")]
        [HttpGet]
        public IActionResult Delete(Guid classGuid)
        {
            TempData["ClassGuid"] = classGuid;
            return PartialView();
        }

        [Authorize(Roles = "Admin, Master")]
        [HttpPost]
        public async Task<IActionResult> Delete()
        {
            if (User.FindFirstValue(ClaimTypes.Role).Equals(UserRole.Master.ToString()))
            {
                var @class = await _classService.GetClass((Guid)TempData["ClassGuid"]);

                if (@class == null || !User.FindFirstValue(ClaimTypes.NameIdentifier).Equals(@class.User.Id))
                {
                    TempData["ToasterState"] = ToasterState.Error;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Constants.ClassDeleteFailed;

                    return RedirectToAction("MyClasses", "Master");
                }
            }

            var result = await _classService.DeleteClass((Guid)TempData["ClassGuid"]);

            if (!result.Succeeded)
            {
                TempData["ToasterState"] = ToasterState.Error;
                TempData["ToasterType"] = ToasterType.Message;
                TempData["ToasterMessage"] = Constants.ClassDeleteFailed;

                if (User.FindFirstValue(ClaimTypes.Role).Equals(UserRole.Admin.ToString()))
                {
                    return RedirectToAction("Index");
                }

                return RedirectToAction("MyClasses", "Master");
            }

            TempData["ToasterState"] = ToasterState.Success;
            TempData["ToasterType"] = ToasterType.Message;
            TempData["ToasterMessage"] = Constants.ClassDeleteSuccessful;

            if (User.FindFirstValue(ClaimTypes.Role).Equals(UserRole.Admin.ToString()))
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("MyClasses", "Master");
        }
    }
}
