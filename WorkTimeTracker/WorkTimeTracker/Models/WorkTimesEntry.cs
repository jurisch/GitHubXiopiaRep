using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkTimeTracker.Models
{
    public class WorkTimesEntry
    {
        public int Id { get; set; }

        public DateTime WorkDay { get; set; }

        public DateTime WorkStartTime { get; set; }

        public DateTime WorkEndTime { get; set; }

        public int PauseLength { get; set; }

        public int EmployeeId { get; set; }

        public int ProjectId { get; set; }
    }
}