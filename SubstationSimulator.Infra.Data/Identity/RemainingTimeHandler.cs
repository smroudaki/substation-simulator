using ElectricalEmulator.Application.Common;
using ElectricalEmulator.Domain.Enums;
using ElectricalEmulator.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Infra.Data.Identity
{
    public class RemainingTimeHandler : AuthorizationHandler<RemainingTimeRequirement>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemainingTimeHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RemainingTimeRequirement requirement)
        {
            var currentUserRole = context.User.FindFirstValue(ClaimTypes.Role);

            if (currentUserRole.Equals(UserRole.Student.ToString()))
            {
                var currentUserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

                var user = await _unitOfWork.Users
                    .GetUser(currentUserId)
                    .SingleOrDefaultAsync();

                if (user.RemainingTime <= 0)
                {
                    context.Fail();
                    return;
                }
            }

            context.Succeed(requirement);
        }
    }
}
