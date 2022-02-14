using ElectricalEmulator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectricalEmulator.Domain.Interfaces
{
    public interface IInterlockRepository : IRepository<Interlock>
    {
        IQueryable<Interlock> GetInterlock(Guid interlockGuid);
    }
}
