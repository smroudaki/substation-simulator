using ElectricalEmulator.Domain.Entities;
using ElectricalEmulator.Domain.Interfaces;
using ElectricalEmulator.Infra.Data.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectricalEmulator.Infra.Data.Repositories
{
    public class InterlockRepository : BaseRepository<Interlock>, IInterlockRepository
    {
        public InterlockRepository(ElectricalEmulatorContext context)
            : base(context)
        {
        }

        public IQueryable<Interlock> GetInterlock(Guid interlockGuid)
        {
            return _context.Interlock
                .Where(i => i.InterlockGuid.Equals(interlockGuid));
        }
    }
}
