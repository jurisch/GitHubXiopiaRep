using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XiopiaWorkTimeTracker.BusinessLogic
{
    public class WorkTimeRow
    {
        public DateTime Date { get; set; }
        public int EntryId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int PauseLength { get; set; }
        public string Project { get; set; }
        public bool WorkDay { get; set; }
    }
}