using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XiopiaWorkTimeTracker.BusinessLogic
{
    public class WorkTimeRow
    {
        public DateTime Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string PauseLength { get; set; }
        public string Project { get; set; }
        public bool WorkDay { get; set; }
    }
}