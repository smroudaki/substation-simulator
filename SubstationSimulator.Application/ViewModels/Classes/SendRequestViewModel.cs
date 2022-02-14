using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ElectricalEmulator.Application.ViewModels.Classes
{
    public class SendRequestViewModel
    {
        [Required]
        public Guid ClassGuid { get; set; }
    }
}
