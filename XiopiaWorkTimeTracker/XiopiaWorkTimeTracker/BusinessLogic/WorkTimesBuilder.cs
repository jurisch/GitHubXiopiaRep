using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XiopiaWorkTimeTracker.Models;

namespace XiopiaWorkTimeTracker.BusinessLogic
{
    public class WorkTimesBuilder
    {
        public static List<WorkTimeRow> GetMonth(int month)
        {
            List<WorkTimeRow> entryList = new List<WorkTimeRow>();
            var daysCount = DateTime.DaysInMonth(DateTime.Now.Year, month);
            for (int i = 1; i <= daysCount; i++)
            {
                entryList.Add(new WorkTimeRow()
                {
                    Date = new DateTime(DateTime.Now.Year, month, i),
                    StartTime = "",
                    EndTime = "",
                    PauseLength = "",
                    Project = ""
                });
            }
            return entryList;

        }
    }
}