using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ElectricalEmulator.Infra.Data.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectricalEmulator.Web.Controllers.Api
{
    public class UserPostController : BaseApiController
    {
        private readonly ElectricalEmulatorContext _context;

        public UserPostController(ElectricalEmulatorContext context)
        {
            _context = context;
        }

        [HttpGet("{postGuid}")]
        public async Task<IActionResult> Get(Guid postGuid)
        {
            var userPost = await _context.UserPost
                .FirstOrDefaultAsync(up => up.UserId.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier)) && up.Post.PostGuid.Equals(postGuid));

            if (userPost == null)
            {
                return NotFound();
            }

            return Ok(userPost);
        }

        [HttpPut("{postGuid}")]
        public async Task<IActionResult> Update(Guid postGuid, object postRawValue)
        {
            var userPost = await _context.UserPost
                .FirstOrDefaultAsync(up => up.UserId.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier)) && up.Post.PostGuid.Equals(postGuid));

            if (userPost == null)
            {
                return NotFound();
            }

            userPost.PostChanges = postRawValue.ToString();
            userPost.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
