using ElectricalEmulator.Application.Common;
using ElectricalEmulator.Application.Common.Extensions;
using ElectricalEmulator.Application.Interfaces;
using ElectricalEmulator.Application.ViewModels.Interlocks;
using ElectricalEmulator.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Application.Services
{
    public class InterlockService : IInterlockService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InterlockService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<InterlockViewModel> GetInterlock(Guid interlockGuid)
        {
            var interlock = await _unitOfWork.Interlocks
                .GetInterlock(interlockGuid)
                .SingleOrDefaultAsync();

            if (interlock == null)
            {
                return null;
            }

            //var interlockJsonResultViewModel = JsonConvert.DeserializeObject<InterlocksJsonResultViewModel>(interlock.RawValue);

            //if (interlockJsonResultViewModel == null)
            //{
            //    return null;
            //}

            return new InterlockViewModel
            {
                InterlockId = interlock.InterlockId,
                InterlockGuid = interlock.InterlockGuid,
                RawValue = interlock.RawValue,
                RawValueDeserialized = new InterlockRawValueViewModel
                {
                    
                },
                CreationDate = PersianDateExtensions.ToPeString(interlock.CreationDate, "yyyy/MM/dd HH:mm"),
                ModifiedDate = interlock.ModifiedDate == null ?
                    Constants.NotSet : PersianDateExtensions.ToPeString(interlock.ModifiedDate.Value, "yyyy/MM/dd HH:mm")
            };
        }
    }
}
