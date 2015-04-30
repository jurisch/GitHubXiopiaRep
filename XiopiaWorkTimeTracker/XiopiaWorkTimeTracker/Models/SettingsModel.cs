using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XiopiaWorkTimeTracker.Models
{
    public class SettingsModel
    {
        public int FirstWorkDayOfWeek { get; set; }
        public int LastWorkDayOfWeek { get; set; }
    }
}