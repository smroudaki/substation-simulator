using ElectricalEmulator.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricalEmulator.Application.ViewModels.Interlocks
{
    public class InterlockViewModel
    {
        public int InterlockId { get; set; }
        public Guid InterlockGuid { get; set; }
        public string RawValue { get; set; }
        public InterlockRawValueViewModel RawValueDeserialized { get; set; }
        public string CreationDate { get; set; }
        public string ModifiedDate { get; set; }
    }
}
