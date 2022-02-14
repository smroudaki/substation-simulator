using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ElectricalEmulator.Domain.Entities
{
    public partial class Interlock
    {
        public int InterlockId { get; set; }
        public Guid InterlockGuid { get; set; }
        public string RawValue { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
