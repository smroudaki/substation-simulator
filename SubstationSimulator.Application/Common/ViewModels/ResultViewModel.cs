using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricalEmulator.Application.Common.ViewModels
{
    public class ResultViewModel
    {
        public ResultViewModel(bool succeeded)
        {
            Succeeded = succeeded;
        }

        public static ResultViewModel Success()
        {
            return new ResultViewModel(true);
        }

        public static ResultViewModel Failure()
        {
            return new ResultViewModel(false);
        }

        public bool Succeeded { get; set; }
    }
}
