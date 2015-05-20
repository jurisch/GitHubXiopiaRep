using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XiopiaWorkTimeTracker.Models.Database;
using XiopiaWorkTimeTracker.Models.Repositories;

namespace XiopiaWorkTimeTracker.Models.ViewModels
{
    public class ResultRowHelper
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string Day { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int PauseLength { get; set; }
        public int ProjectId { get; set; }
        public bool AttrIll { get; set; }
        public bool AttrHoliday { get; set; }

        public WorkTimeEntry GetWorkTimeEntry()
        {
            ProjectsRepository projRepo = new ProjectsRepository();
            WorkTimeEntry wtEntry = new WorkTimeEntry();
            string[] dayStrArr = null;
            string[] howMinStartStr = null;
            string[] howMinEndStr = null;

            if (!string.IsNullOrEmpty(this.Day))
            {
                dayStrArr = this.Day.Split('.');
            }
            if (!string.IsNullOrEmpty(this.StartTime))
            {
                howMinStartStr = this.StartTime.Split(':');
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                howMinEndStr = this.EndTime.Split(':');
            }
            wtEntry.WorkDay = new DateTime(this.Year, this.Month, Int32.Parse(dayStrArr[0]));
            if (howMinStartStr != null)
            {
                wtEntry.WorkStartTime = new DateTime(this.Year, this.Month, Int32.Parse(dayStrArr[0]), Int32.Parse(howMinStartStr[0]), Int32.Parse(howMinStartStr[1]), 0);
            }
            if (howMinEndStr != null)
            {
                wtEntry.WorkEndTime = new DateTime(this.Year, this.Month, Int32.Parse(dayStrArr[0]), Int32.Parse(howMinEndStr[0]), Int32.Parse(howMinEndStr[1]), 0);
            }
            if (this.ProjectId != 0)
            {
                wtEntry.ProjectName = projRepo.Get(this.ProjectId).Name;
            }
            wtEntry.PauseLength = this.PauseLength;
            wtEntry.AttrIll = this.AttrIll;
            wtEntry.AttrHoliday = this.AttrHoliday;
            return wtEntry;
        }

    }

}