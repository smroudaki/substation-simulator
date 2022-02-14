using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricalEmulator.Infra.Data.Identity
{
    public class RemainingTimeRequirement : IAuthorizationRequirement
    {
        public int MinimumRemainingTime { get; }

        public RemainingTimeRequirement(int minimumRemainingTime)
        {
            MinimumRemainingTime = minimumRemainingTime;
        }
    }
}
