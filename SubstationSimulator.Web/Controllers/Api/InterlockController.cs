using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectricalEmulator.Application.ViewModels.Interlocks;
using ElectricalEmulator.Domain.Entities;
using ElectricalEmulator.Domain.Interfaces;
using ElectricalEmulator.Infra.Data.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ElectricalEmulator.Web.Controllers.Api
{
    public class InterlockController : BaseApiController
    {
        private readonly ElectricalEmulatorContext _context;

        public InterlockController(ElectricalEmulatorContext context)
        {
            _context = context;
        }

        // [HttpGet("{interlock}")]
        // public async Task<IActionResult> GetById(Guid interlockGuid)
        // {
        //     if (interlockGuid == null)
        //         return BadRequest();

        //     var interlocks = await _context.Interlock
        //         .SingleOrDefaultAsync(i => i.InterlockGuid == interlockGuid);

        //     if (interlocks == null)
        //         return NotFound();

        //     var interlockJsonResultViewModel = JsonConvert.DeserializeObject<InterlocksJsonResultViewModel>
        //         (interlocks.RawValue);

        //     List<InterlockViewModel> interlocksViewModel = new List<InterlockViewModel>();

        //     foreach (var interlock in interlockJsonResultViewModel.Interlocks)
        //     {
        //         var interlockViewModel = new InterlockViewModel
        //         {
        //             Name = interlock.Name,
        //             TypeCode = interlock.TypeCode,
        //             LocX = interlock.LocX,
        //             LocY = interlock.LocY,
        //             RotZ = interlock.RotZ
        //         };

        //         interlocksViewModel.Add(interlockViewModel);
        //     }

        //     if (interlocksViewModel.Count <= 0)
        //         return NoContent();

        //     return Ok(interlocksViewModel);
        // }

        [HttpGet("{interlockGuid}")]
        public async Task<IActionResult> Get(Guid interlockGuid)
        {
            var interlock = await _context.Interlock
                .SingleOrDefaultAsync(i => i.InterlockGuid.Equals(interlockGuid));

            if (interlock == null)
            {
                return NotFound();
            }

            return Ok(interlock);
        }

        [HttpPost]
        public async Task<string> Create(object rawData)
        {
            if (rawData == null)
            {
                return null;
            }

            var interlock = new Interlock
            {
                RawValue = rawData.ToString()
            };

            _context.Interlock.Add(interlock);
            await _context.SaveChangesAsync();

            Response.StatusCode = StatusCodes.Status201Created;

            return interlock.InterlockGuid.ToString();

            //return new CreateIntelockViewModel
            //{
            //    InterlockGuid = interlock.InterlockGuid
            //};
        }
    }
}
