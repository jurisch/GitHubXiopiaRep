using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XiopiaWorkTimeTracker.Models.ViewModels
{
    public class OverviewViewModel
    {
        private int _userId;

        public OverviewViewModel(int userId)
        {
            _userId = userId;
        }

        public int WorkdaysInMonth {
            get
            {
                return 20;
            }
        }
        public int WorkdaysYear { get; set; }
        public int WorkedNormalDays { get; set; }
        public int WorkedOtherDays { get; set; }
    }
}