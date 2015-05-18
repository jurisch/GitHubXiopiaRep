using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XiopiaWorkTimeTracker.Models.Database
{
    public class WorkTimeEntry
    {
        public int Id { get; set; }

        public DateTime WorkDay { get; set; }

        public DateTime? WorkStartTime { get; set; }

        public DateTime? WorkEndTime { get; set; }

        public int? PauseLength { get; set; }

        public int? EmployeeId { get; set; }

        public string ProjectName { get; set; }

        public bool AttrIll { get; set; }

        public bool AttrHoliday { get; set; }
    }
}