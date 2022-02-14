using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricalEmulator.Domain.Entities
{
    public partial class Report
    {
        public int ReportId { get; set; }
        public Guid ReportGuid { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        public int? ClassId { get; set; }
        public string Value { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual Class Class { get; set; }
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
