using ElectricalEmulator.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ElectricalEmulator.Web.Controllers.Api
{
    public class CurrentUserController : BaseApiController
    {
        private readonly IUserService _userService;

        public CurrentUserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetRemainingTime()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.GetUser(currentUserId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.RemainingTime);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> DecreaseRemainingTime()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _userService.DecreaseUserRemainingTime(currentUserId);

            if (result == -1)
            {
                return NotFound();
            }
            else if (result == -2)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}
