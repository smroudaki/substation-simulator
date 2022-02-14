using ElectricalEmulator.Domain.Entities;
using ElectricalEmulator.Domain.Interfaces;
using ElectricalEmulator.Infra.Data.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectricalEmulator.Infra.Data.Repositories
{
    public class SettingRepository : BaseRepository<Setting>, ISettingRepository
    {
        public SettingRepository(ElectricalEmulatorContext context)
            : base(context)
        {
        }

        public IQueryable<Setting> GetSettings()
        {
            return _context.Setting.AsQueryable();
        }
    }
}
