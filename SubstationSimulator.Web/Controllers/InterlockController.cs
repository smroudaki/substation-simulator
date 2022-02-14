using ElectricalEmulator.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectricalEmulator.Web.Controllers
{
    public class InterlockController : Controller
    {
        private readonly IInterlockService _interlockService;

        public InterlockController(IInterlockService interlockService)
        {
            _interlockService = interlockService;
        }

        [Authorize(Roles = "Admin, Master, Student")]
        [HttpGet]
        public async Task<string> Index(Guid interlockGuid)
        {
            var interlock = await _interlockService.GetInterlock(interlockGuid);
            return interlock == null ? null : interlock.RawValue;
        }
    }
}
